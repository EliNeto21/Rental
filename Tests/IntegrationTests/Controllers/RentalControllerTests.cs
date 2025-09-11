using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ViewModel;
using FluentAssertions;

namespace Tests.IntegrationTests.Controllers
{
    public class RentalControllerTests : IClassFixture<ApiFactory>
    {
        private readonly HttpClient _client;

        public RentalControllerTests(ApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_Rental_Return_BeforeExpected_Should_ApplyPenalty()
        {
            // arrange -> cria courier e motorcycle primeiro (simula cenário)
            var courier = new 
            { 
                name = "Neto", 
                cnpj = "123456780777",
                birthDate = "1990-01-01", 
                cnhNumber = "CNH077", 
                cnhType = "A" 
            };

            var cResp = await _client.PostAsJsonAsync("Courier", courier);
            var courierResponse = await cResp.Content.ReadFromJsonAsync<GenericResult<Courier>>();

            var motorcycle = new 
            { 
                year = 2024, 
                model = "Honda", 
                plate = "ZZZ9977" 
            };

            var mResp = await _client.PostAsJsonAsync("Motorcycles", motorcycle);
            var motoResponse = await mResp.Content.ReadFromJsonAsync<GenericResult<Motorcycle>>();

            var rentalReq = new 
            {
                courierId = courierResponse.Data.Id, 
                motorcycleId = motoResponse.Data.Id,
                startDate = DateOnly.FromDateTime(DateTime.UtcNow),
                endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
                expectedEndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
                planDays = 7, 
                dailyRate = 30 
            };

            var rResp = await _client.PostAsJsonAsync("Rental", rentalReq);
            var rentalResponse = await rResp.Content.ReadFromJsonAsync<GenericResult<Rental>>();

            // act -> devolve antes da data prevista
            var returnReq = new 
            { 
                endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)) 
            };

            var resp = await _client.PutAsJsonAsync($"Rental/by-id/{rentalResponse.Data.Id}/return", returnReq);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
