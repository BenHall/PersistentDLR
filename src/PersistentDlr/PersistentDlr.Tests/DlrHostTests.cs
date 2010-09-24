using NUnit.Framework;
using PersistentDlr.Dlr;

namespace PersistentDlr.Tests
{
    [TestFixture]
    public class DlrHostTests
    {
        [Test]
        public void Execute_with_1_plus_1_should_return_2() {
            var host = new DlrHost();
            var response = host.Execute("1+1");
            Assert.That(response.ToString(), Is.EqualTo("2"));
        }

        [Test]
        public void Scope_persists_across_execute_calls()
        {
            var host = new DlrHost();
            
            host.Execute("i = 8");
            
            var response = host.Execute("i");

            Assert.That(response.ToString(), Is.EqualTo("8"));
        }
    }
}