﻿using System;
using System.IO;
using NUnit.Framework;
using SquishIt.Framework.Files;

namespace SquishIt.Tests
{
    [TestFixture]
    public class RetryableFileOpenerTest
    {
        [Test]
        public void OpenTextStreamWriter_Throws_Exception_With_Filepath()
        {
            var fakePath = Guid.NewGuid().ToString();

            var opener = new RetryableFileOpener();

            var ex = Assert.Throws<IOException>(() => opener.OpenTextStreamWriter(fakePath, 0, true));

            Assert.True(ex.Message.Contains(fakePath));
        }

        [Test]
        public void OpenTextStreamReader_Throws_Exception_With_Filepath()
        {
            var fakePath = Guid.NewGuid().ToString();

            var opener = new RetryableFileOpener();

            var ex = Assert.Throws<IOException>(() => opener.OpenTextStreamReader(fakePath, 0));

            Assert.True(ex.Message.Contains(fakePath));
        }

        [Test]
        public void OpenFileStream_Throws_Exception_With_Filepath()
        {
            var fakePath = Guid.NewGuid().ToString();

            var opener = new RetryableFileOpener();

            var ex = Assert.Throws<IOException>(() => opener.OpenFileStream(fakePath, 0, FileMode.Open, FileAccess.ReadWrite, FileShare.None));

            Assert.True(ex.Message.Contains(fakePath));
        }
    }
}
