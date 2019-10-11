using NUnit.Framework;
using Shouldly;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FunSharp;
using static FunSharp.TryFunctions;

namespace FunSharp.Tests
{
    public class OptTests
    {
        [Test]
        public void Deve_Ser_None_Objeto_Nulo()
        {
            string word = null;
            var opt1 = Opt.Of(word);
            opt1.IsNone.ShouldBeTrue();
            opt1.IsSome.ShouldBeFalse();

            Tipo tipo = null;
            var opt2 = Opt.Of(tipo);
            opt2.IsNone.ShouldBeTrue();
            opt2.IsSome.ShouldBeFalse();
        }

        [Test]
        public void Deve_Ser_None_ValueType_Default()
        {
            int numero = 0;
            var opt1 = Opt.Of(numero);
            opt1.IsNone.ShouldBeTrue();
            opt1.IsSome.ShouldBeFalse();

            Enumeracao enumeracao = 0;
            var opt2 = Opt.Of(enumeracao);
            opt2.IsNone.ShouldBeTrue();
            opt2.IsSome.ShouldBeFalse();

            decimal valor = 0M;
            var opt3 = Opt.Of(valor);
            opt3.IsNone.ShouldBeTrue();
            opt3.IsSome.ShouldBeFalse();

            char carac = '\0';
            var opt4 = Opt.Of(carac);
            opt4.IsNone.ShouldBeTrue();
            opt4.IsSome.ShouldBeFalse();

            bool booleano = false;
            var opt5 = Opt.Of(booleano);
            opt5.IsNone.ShouldBeTrue();
            opt5.IsSome.ShouldBeFalse();

            Point ponto = default;
            var opt6 = Opt.Of(ponto);
            opt6.IsNone.ShouldBeTrue();
            opt6.IsSome.ShouldBeFalse();
        }

        [Test]
        public void Deve_Retornar_Valor()
        {
            Tipo tipo = new Tipo() { Numero = 10 };
            var opt1 = Opt.Of(tipo);
            var retorno1 = opt1.GetValueOrElse(new Tipo() { Numero = 0 });
            retorno1.ShouldNotBeNull();
            retorno1.Numero.ShouldBe(10);

            var opt2 = Opt.Of(100);
            var retorno2 = opt2.GetValueOrElse(0);
            retorno2.ShouldNotBe(0);
            retorno2.ShouldBe(100);
        }

        [Test]
        public void Deve_Retornar_Valor_Match_Correto()
        {
            Tipo tipo = new Tipo() { Numero = 10 };
            var opt1 = Opt.Of(tipo);
            int retorno1 = opt1.Match(some: t => t.Numero, none: _ => 0);
            retorno1.ShouldBe(10);

            Tipo nulo = null;
            var opt2 = Opt.Of(nulo);
            int retorno2 = opt2.Match(some: t => 10, none: _ => 0);
            retorno2.ShouldBe(0);

            char letra = 'A';
            var opt3 = Opt.Of(letra);
            char retorno3 = opt3.Match(some: c => c, none: _ => '\0');
            retorno3.ShouldBe('A');

            char zero = '\0';
            var opt4 = Opt.Of(zero);
            char retorno4 = opt4.Match(some: c => c, none: _ => '\0');
            retorno4.ShouldBe('\0');
        }

        [Test]
        public void Deve_Fazer_Cast_Implicito_Para_Opt()
        {
            Opt<int> opt1 = 134;
            opt1.IsSome.ShouldBeTrue();
            opt1.IsNone.ShouldBeFalse();
            opt1.GetValueOrElse(0).ShouldBe(134);

            Opt<Tipo> opt2 = new Tipo() { Numero = 200 };
            opt2.IsSome.ShouldBeTrue();
            opt2.IsNone.ShouldBeFalse();
            opt2.GetValueOrElse(new Tipo() { Numero = -1 }).Numero.ShouldBe(200);

            Opt<Enumeracao> opt3 = Enumeracao.ItemTres;
            opt3.IsSome.ShouldBeTrue();
            opt3.IsNone.ShouldBeFalse();
            opt3.GetValueOrElse(0).ShouldBe(Enumeracao.ItemTres);

            Opt<decimal> opt4 = None.Instance;
            opt4.IsSome.ShouldBeFalse();
            opt4.IsNone.ShouldBeTrue();

            Tipo tipoNulo = null;
            Opt<Tipo> opt5 = tipoNulo;
            opt5.IsSome.ShouldBeFalse();
            opt5.IsNone.ShouldBeTrue();
        }

    }

    enum Enumeracao
    {
        ItemUm, ItemDois, ItemTres
    }

    struct Point
    { }

    class Tipo
    { 
        public int Numero { get; set; }
    }
}
