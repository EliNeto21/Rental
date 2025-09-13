using System.Net;
using System.Net.Http.Json;
using Domain.Entities;
using Domain.ViewModel;
using FluentAssertions;
using Tests.IntegrationTests.Helpers;

namespace Tests.IntegrationTests.Controllers
{
    public class CourierControllerTests : IntegrationTestBase
    {
        public CourierControllerTests(PostgresFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Post_Courier_Should_Return200()
        {
            var request = new
            {
                name = "Eli Neto",
                cnpj = "1234567777",
                birthDate = "1995-05-01",
                cnhNumber = "CNH-777",
                cnhType = "A"
            };

            var response = await Client.PostAsJsonAsync("/Courier", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Post_Courier_DuplicateCnpj_Should_Return400()
        {
            var request = new
            {
                name = "João",
                cnpj = "111111110777",
                birthDate = "1990-01-01",
                cnhNumber = "CNH-888",
                cnhType = "A+B"
            };

            // cria o primeiro
            await Client.PostAsJsonAsync("Courier", request);

            // tenta criar duplicado
            var response = await Client.PostAsJsonAsync("Courier", request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_Courier_DuplicateCnhNumber_Should_Return400()
        {
            var courier1 = new
            {
                name = "Maria",
                cnpj = "222222220054654",
                birthDate = "1988-02-02",
                cnhNumber = "CNH-564",
                cnhType = "B"
            };

            var courier2 = new
            {
                name = "José",
                cnpj = "33333335497964",
                birthDate = "1992-03-03",
                cnhNumber = "CNH-564", // duplicado
                cnhType = "A"
            };

            await Client.PostAsJsonAsync("Courier", courier1);
            var response = await Client.PostAsJsonAsync("Courier", courier2);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        //[Fact]
        //public async Task Post_CourierCnhImage_Should_RejectInvalidFormat()
        //{
        //    var request = new
        //    {
        //        name = "Ana",
        //        cnpj = "55555555000199",
        //        birthDate = "1985-05-05",
        //        cnhNumber = "CNH-006",
        //        cnhType = "A+B"
        //    };



        //    var cResp = await _client.PostAsJsonAsync("Courier", request);
        //    var body = await cResp.Content.ReadFromJsonAsync<GenericResult<Courier>>();

        //    var content = new MultipartFormDataContent();
        //    var fileContent = new ByteArrayContent(new byte[] { 65, 66, 67 });
        //    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
        //    content.Add(fileContent, "file", "cnh.txt");

        //    var response = await _client.PostAsync($"Courier/{body.Data.Id}/cnh-image", content);

        //    // assert
        //    response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        //}
    }
}
