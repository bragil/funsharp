using FunSharp.Tasks;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FunSharp.Tests.Tasks
{
    [TestFixture]
    public class TaskTests
    {
        [Test]
        public async Task Deve_Obter_Valor_Task_Apos_Then()
        {
            var res = await Task.FromResult(128)
                                .Then(n => (n*2).ToString());

            Assert.AreEqual("256", res);
        }

        [Test]
        public async Task Deve_Obter_Valor_Task_Apos_Segundo_Then()
        {
            var res = await Task.FromResult(128)
                                .Then(n => (n * 2).ToString())
                                .Then(s => long.Parse(s));

            Assert.AreEqual(256L, res);
        }

        [Test]
        public async Task Deve_Obter_Valor_Task_Com_Match()
        {
            var res = await Task.FromResult("bla")
                                .Match<string>(
                                    str => str,
                                    _ => "none"
                                );
            Assert.AreEqual("bla", res);


            var res2 = await Task.FromResult(1259.75M)
                                    .Match(
                                        num => "1259.75",
                                        _ => "0"
                                    );
            Assert.AreEqual("1259.75", res2);
        }

    }
}
