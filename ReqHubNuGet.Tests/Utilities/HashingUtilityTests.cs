using ReqHub;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ReqHubNuGet.Tests.Utilities
{
    public class HashingUtilityTests
    {
        private readonly string publicKey = "test.public";
        private readonly string privateKey = "test.private";

        [Fact]
        public void TestCreate()
        {
            var tokenResult = HashingUtility.Create(this.publicKey, this.privateKey);

            Assert.NotNull(tokenResult.token);
            Assert.NotNull(tokenResult.timestamp);
            Assert.NotNull(tokenResult.nonce);
        }

        [Fact]
        public void TestCreate_WithTimestamp()
        {
            var tokenResult = HashingUtility.Create(this.publicKey, this.privateKey, timestamp: "5");

            Assert.NotNull(tokenResult.token);
            Assert.Equal("5", tokenResult.timestamp);
            Assert.NotNull(tokenResult.nonce);
        }

        [Fact]
        public void TestCreate_WithNonce()
        {
            var tokenResult = HashingUtility.Create(this.publicKey, this.privateKey, nonce: "test");

            Assert.NotNull(tokenResult.token);
            Assert.NotNull(tokenResult.timestamp);
            Assert.Equal("test", tokenResult.nonce);
        }

        [Fact]
        public void TestGenerateKey()
        {
            var key = HashingUtility.GenerateKey(size: 64);

            Assert.NotNull(key);
        }

        [Fact]
        public void TestHash()
        {
            var hash = HashingUtility.Hash(this.publicKey, this.privateKey);

            Assert.NotNull(hash);
        }
    }
}
