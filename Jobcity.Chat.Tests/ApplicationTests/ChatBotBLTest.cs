using Jobcity.Chat.Bot.Contracts;
using Jobcity.Chat.Bot.Implementations;
using Jobcity.Chat.InfraLayer.Constants;
using Jobcity.Chat.InfraLayer.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobcity.Chat.Tests.ApplicationTests
{
    [TestClass]
    public class ChatBotBLTest
    {
        private readonly IChatBotBL _chatBotBL;
        private readonly Mock<IChatApiProxy> _mockChatApiProxy;
        private readonly Mock<IChatBotRabbitProxy> _mockChatBotRabbitProxy;

        public ChatBotBLTest()
        {
            _mockChatApiProxy = new Mock<IChatApiProxy>();
            _mockChatBotRabbitProxy = new Mock<IChatBotRabbitProxy>();
            _chatBotBL = new ChatBotBL(_mockChatApiProxy.Object, _mockChatBotRabbitProxy.Object);
        }

        [TestMethod]
        public async Task Given_ValidParameters_When_CallChatBotToGetStockCode_Then_ReturnsSuccessMessage()
        {
            // Arrange
            var expectedMessage = "APPL.US quote is $93.42 per share.";
            var stockCode = $"{Commands.StockCode}{Commands.AaplUsCode}";
            _mockChatApiProxy.Setup(x => x.DownloadCsv(Commands.AaplUsCode))
                .ReturnsAsync(expectedMessage);
            _mockChatBotRabbitProxy.Setup(x => x.SendMessage(It.IsAny<string>())).Verifiable();

            // Act
            var message = await _chatBotBL.CallChatBotToGetStockCode(stockCode);

            // Assert
            Assert.AreEqual(expectedMessage, message);
        }

        [TestMethod]
        public async Task Given_InvalidParameters_When_CallChatBotToGetStockCode_Then_ReturnsNoSucesssMessage()
        {
            // Arrange
            var expectedMessage = BotMessages.StockCodeNotAvailable;
            var stockCode = string.Empty;
            _mockChatApiProxy.Setup(x => x.DownloadCsv(stockCode))
                .ReturnsAsync(expectedMessage);
            _mockChatBotRabbitProxy.Setup(x => x.SendMessage(It.IsAny<string>())).Verifiable();

            // Act
            var message = await _chatBotBL.CallChatBotToGetStockCode(stockCode);

            // Assert
            Assert.AreEqual(expectedMessage, message);
        }
    }
}
