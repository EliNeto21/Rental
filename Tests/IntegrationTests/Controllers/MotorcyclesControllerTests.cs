using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Domain.Entities;
using Domain.ViewModel;

namespace Tests.IntegrationTests.Controllers
{
    public class MotorcyclesControllerTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public MotorcyclesControllerTests(ApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_Motorcycle_Should_Return200()
        {
            var request = new
            {
                year = 2024,
                model = "Honda CG 160",
                plate = "ABC1234"
            };

            var motorcycle = await _client.GetFromJsonAsync<GenericResult<IReadOnlyList<Motorcycle>>> ($"/Motorcycles/by-plate/{request.plate}");
            await _client.DeleteAsync($"/Motorcycles/{motorcycle.Data.FirstOrDefault().Id}");

            var response = await _client.PostAsJsonAsync("/Motorcycles", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadFromJsonAsync<GenericResult<Motorcycle>>();

            (body.Data.Year).Should().Be(2024);
            (body.Data.Plate).Should().Be("ABC1234");
        }

        [Fact]
        public async Task Post_Motorcycle_DuplicatePlate_Should_Return400()
        {
            var request = new 
            { 
                year = 2024, 
                model = "Yamaha", 
                plate = "XYZ9876" 
            };

            // cria primeira
            await _client.PostAsJsonAsync("Motorcycles", request);

            // tenta criar segunda
            var response = await _client.PostAsJsonAsync("Motorcycles", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_Motorcycle_OldYar_Should_Return400()
        {
            var request = new
            {
                year = 1979,
                model = "Yamaha",
                plate = "XYZ9876"
            };

            var response = await _client.PostAsJsonAsync("Motorcycles", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
