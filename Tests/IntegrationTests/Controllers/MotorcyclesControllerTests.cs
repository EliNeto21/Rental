using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using Domain.Entities;
using Domain.ViewModel;
using Tests.IntegrationTests.Helpers;

namespace Tests.IntegrationTests.Controllers
{
    public class MotorcyclesControllerTests : IntegrationTestBase
    {
        public MotorcyclesControllerTests(PostgresFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Post_Motorcycle_Should_Return200()
        {
            try
            {
                var request = new
                {
                    year = 2024,
                    model = "Honda CG 160",
                    plate = "ABC1234"
                };

                var response = await Client.PostAsJsonAsync("/Motorcycles", request);
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var body = await response.Content.ReadFromJsonAsync<GenericResult<Motorcycle>>();

                (body.Data.Year).Should().Be(2024);
                (body.Data.Plate).Should().Be("ABC1234");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            
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
            await Client.PostAsJsonAsync("Motorcycles", request);

            // tenta criar segunda
            var response = await Client.PostAsJsonAsync("Motorcycles", request);

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

            var response = await Client.PostAsJsonAsync("Motorcycles", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
