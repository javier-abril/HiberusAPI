using HiberusAPINegocio;
using HiberusAPIEntidades;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace Tests
{
    public class Tests
    {
        private Mock<INegocio> mockNegocio;
        private List<RateEnt> mockListRates;
        private List<Transaction> mockListTransactions;

        [SetUp]
        public void Setup()
        {
            mockNegocio = new Mock<INegocio>();
            rellenoRatesList();
            rellenoTransactionsList();

            mockNegocio.Setup(ng => ng.GetRates()).Returns(mockListRates);
            mockNegocio.Setup(ng => ng.GetTransactions()).Returns(mockListTransactions);

        }

        private void rellenoTransactionsList()
        {
            mockListTransactions = new List<Transaction>();

            for (int i=1; i < 10; i++)
            {
                Transaction tr = new Transaction
                {
                    Id = i,
                    Amount = 1 * i,
                    Currency = "EUR",
                    Sku = "TR300" + i
                };
                mockListTransactions.Add(tr);
            }
        }

        private void rellenoRatesList()
        {
            mockListRates = new List<RateEnt>();
            RateEnt rate = new RateEnt
            {
                Id = 1,
                From = "EUR",
                To = "USD",
                Rate = 1.2m
            };
            mockListRates.Add(rate);

            rate = new RateEnt
            {
                Id = 2,
                From = "USD",
                To = "EUR",
                Rate = 0.8m
            };
            mockListRates.Add(rate);

            rate = new RateEnt
            {
                Id = 3,
                From = "USD",
                To = "GBR",
                Rate = 0.6m
            };
            mockListRates.Add(rate);

        }

        [Test]
        public void TestGetRatesIsOK()
        {
            //Arrange
            Negocio negocio = new Negocio(new Logger(),mockListRates,mockListTransactions);
            //Act
            List<RateEnt> rateResult = (List<RateEnt>) negocio.GetRates();
            //Assert
            Assert.AreEqual(3, rateResult.Count, "No se ha obtenido el número esperado de rates");
        }

        [Test]
        public void TestGetTransactionsIsOK()
        {
            //Arrange
            Negocio negocio = new Negocio(new Logger(), mockListRates, mockListTransactions);
            //Act
            List<Transaction> transactionResult = (List<Transaction>)negocio.GetTransactions();
            //Assert
            Assert.AreEqual(9, transactionResult.Count, "No se ha obtenido el número esperado de transactions");
        }
    }
}