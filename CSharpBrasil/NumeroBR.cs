using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using System.Globalization;
using System.Threading;

namespace CSharpBrasil
{
    public class NumeroBR
    {
        private const string NUMERO_NEGATIVO = "Número não pode ser negativo";
        private readonly ResourceManager resourceManager;
        public NumeroBR()
        {
            resourceManager = new ResourceManager(@"CSharpBrasil.Properties.Resources",
                         System.Reflection.Assembly.Load(new System.Reflection.AssemblyName("CSharpBrasil")));
        }

        public string Extenso(int numero)
        {
            if (numero < 0)
            {
                throw new ArgumentOutOfRangeException(NUMERO_NEGATIVO);
            }
            else
            {
                int casa = 1;
                Digito digito = null;
                do
                {
                    switch (casa)
                    {
                        case 1:
                            digito = new DigitoUnidade(numero % 10, casa, digito);
                            break;
                        case 2:
                            digito = new DigitoDezena(numero % 10, casa, digito);
                            break;
                        case 3:
                            digito = new DigitoCentena(numero % 10, casa, digito);
                            break;
                    }
                    casa++;
                    numero /= 10;
                } while (numero > 0);

                return digito.Extenso();
            }
        }

        private string Extenso21_999(int numero)
        {
            double numeroDigitos = Math.Floor(Math.Log10(numero));
            int potenciaDe10 = (int)Math.Pow(10, (int)numeroDigitos);
            if (numero % potenciaDe10 == 0)
            {
                return resourceManager.GetString(string.Format("Extenso{0:000}", numero));
            }
            else
            {
                string estaCasaPorExtenso = string.Empty;
                int estaCasa = (int)((numero / potenciaDe10) * potenciaDe10);
                if (estaCasa == 100)
                    estaCasaPorExtenso = resourceManager.GetString("Extenso100mais");
                else
                    estaCasaPorExtenso = resourceManager.GetString(string.Format("Extenso{0:000}", estaCasa));

                var proximasCasas = numero % potenciaDe10;
                return string.Format("{0:000} e {1:000}"
                    , estaCasaPorExtenso
                    , Extenso(proximasCasas));
            }
        }

        private string Extenso0_20(int numero)
        {
            return resourceManager.GetString(string.Format("Extenso{0:000}", numero));
        }
    }

    abstract class Digito
    {
        protected readonly int _numero;
        protected readonly int _casa;
        protected readonly Digito _digitoFilho;

        public int Numero { get { return _numero; } }

        public Digito(int numero, int casa, Digito digitoFilho)
        {
            _numero = numero;
            _casa = casa;
            _digitoFilho = digitoFilho;
        }

        public virtual string Extenso()
        {
            return ResourceManagerHelper
                .Instance
                .ResourceManager
                .GetString(string.Format("Extenso{0:000}", ValorSemFilho()));
        }

        protected int ValorSemFilho()
        {
            return _numero * (int)Math.Pow(10, _casa - 1);
        }

        protected int ValorFilhos()
        {
            if (_digitoFilho == null)
            {
                return 0;
            }
            else
            {
                return _digitoFilho.ValorTotal();
            }
        }

        protected int ValorTotal()
        {
            int result = ValorSemFilho();
            if (_digitoFilho != null)
                result += _digitoFilho.ValorTotal();
            return result;
        }

        protected string Extenso(int numero)
        {
            return ResourceManagerHelper
                .Instance
                .ResourceManager
                .GetString(string.Format("Extenso{0:000}", numero));
        }
    }

    class DigitoUnidade : Digito
    {
        public DigitoUnidade(int numero, int casa, Digito digitoFilho) : base(numero, casa, digitoFilho) { }
    }

    class DigitoDezena : Digito
    {
        public DigitoDezena(int numero, int casa, Digito digitoFilho) : base(numero, casa, digitoFilho) { }

        public override string Extenso()
        {
            if (_numero == 0)
            {
                return _digitoFilho.Extenso();
            }
            else if (_numero == 1)
            {
                return base.Extenso(ValorSemFilho() + _digitoFilho.Numero);
            }
            else
            {
                if (_digitoFilho.Numero == 0)
                    return base.Extenso(ValorSemFilho());
                else
                    return
                        string.Format("{0} e {1}"
                        , base.Extenso()
                        , _digitoFilho.Extenso());
            }
        }
    }

    class DigitoCentena : Digito
    {
        public DigitoCentena(int numero, int casa, Digito digitoFilho) : base(numero, casa, digitoFilho) { }

        public override string Extenso()
        {
            if (ValorFilhos() == 0)
                return base.Extenso(ValorSemFilho());
            else
            {
                string esteDigitoExtenso = string.Empty;
                if (_numero == 1)
                    esteDigitoExtenso = ResourceManagerHelper.Instance.ResourceManager.GetString("Extenso100mais");
                else
                    esteDigitoExtenso = base.Extenso();

                return
                    string.Format("{0} e {1}"
                    , esteDigitoExtenso
                    , _digitoFilho.Extenso());
            }
        }
    }

    class ResourceManagerHelper
    {
        private readonly ResourceManager resourceManager;
        private static ResourceManagerHelper instance;
        private ResourceManagerHelper()
        {
            resourceManager = new ResourceManager(@"CSharpBrasil.Properties.Resources",
             System.Reflection.Assembly.Load(new System.Reflection.AssemblyName("CSharpBrasil")));
        }

        public static ResourceManagerHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new ResourceManagerHelper();
                return instance;
            }
        }

        public ResourceManager ResourceManager
        {
            get
            {
                return resourceManager;
            }
        }
    }
}
