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
        private readonly ResourceManager resourceManager;
        public NumeroBR()
        {
            resourceManager = new ResourceManager(@"CSharpBrasil.Properties.Resources",
                         System.Reflection.Assembly.Load(new System.Reflection.AssemblyName("CSharpBrasil")));
        }

        public string Extenso(double numero)
        {
            return Extenso(numero);
        }

        public string Extenso(int numero)
        {
            int posicao = 1;
            Grupo grupo = null;
            do
            {
                grupo = new Grupo(numero % 1000, posicao, grupo);
                posicao++;
                numero /= 1000;
            } while (numero > 0);

            return grupo.Extenso();
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

    class Grupo
    {
        private const string NUMERO_NEGATIVO = "Número não pode ser negativo";
        private readonly int _numero;
        protected readonly int _posicao;
        private readonly Digito _digito;
        private readonly Grupo _grupoFilho;
        public Grupo(int numero, int posicao, Grupo grupoFilho)
        {
            if (numero < 0)
            {
                throw new ArgumentOutOfRangeException(NUMERO_NEGATIVO);
            }
            else
            {
                _numero = numero;
                _posicao = posicao;
                _grupoFilho = grupoFilho;
                int posicaoDigito = 1;
                Digito digito = null;
                do
                {
                    switch ((posicaoDigito - 1) % 3)
                    {
                        case 0:
                            digito = new DigitoUnidade(numero % 10, posicaoDigito, digito);
                            break;
                        case 1:
                            digito = new DigitoDezena(numero % 10, posicaoDigito, digito);
                            break;
                        case 2:
                            digito = new DigitoCentena(numero % 10, posicaoDigito, digito);
                            break;
                    }
                    posicaoDigito++;
                    numero /= 10;
                } while (numero > 0);

                _digito = digito;
            }
        }

        protected int ValorSomenteDoGrupo()
        {
            return _digito.ValorTotal();
        }

        protected int ValorDosFilhos()
        {
            if (_grupoFilho == null)
            {
                return 0;
            }
            else
            {
                return _grupoFilho.ValorTotal();
            }
        }

        public int ValorTotal()
        {
            int result = ValorSomenteDoGrupo();
            if (_grupoFilho != null)
                result += _grupoFilho.ValorTotal();
            return result;
        }

        public string Extenso()
        {
            if (_grupoFilho == null)
                return _digito.Extenso();
            else
            {
                int valorGrupo = _digito.ValorTotal();
                string singularPlural = valorGrupo < 2 ? "singular" : "plural";
                string nomeGrupo =
                    ResourceManagerHelper
                        .Instance
                        .ResourceManager
                        .GetString(string.Format("Extenso1e{0}{1}", (_posicao - 1) * 3, singularPlural));

                int valorGrupoFilho = _grupoFilho.ValorTotal();
                string separador = 
                    _grupoFilho.ValorDosFilhos() == 0 ? " e" : ",";

                if (valorGrupoFilho == 0)
                    return string.Format("{0} {1}",
                    _digito.Extenso(),
                    nomeGrupo);
                else
                    return string.Format("{0} {1}{2} {3}",
                    _digito.Extenso(),
                    nomeGrupo,
                    separador,
                    _grupoFilho.Extenso());
            }
        }
    }

    abstract class Digito
    {
        protected readonly int _numero;
        protected readonly int _posicao;
        protected readonly Digito _digitoFilho;

        public int Numero { get { return _numero; } }

        public Digito(int numero, int posicao, Digito digitoFilho)
        {
            _numero = numero;
            _posicao = posicao;
            _digitoFilho = digitoFilho;
        }

        public virtual string Extenso()
        {
            return ResourceManagerHelper
                .Instance
                .ResourceManager
                .GetString(string.Format("Extenso{0:000}", ValorSomenteDoDigito()));
        }

        protected int ValorSomenteDoDigito()
        {
            return _numero * (int)Math.Pow(10, _posicao - 1);
        }

        protected int ValorDosFilhos()
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

        public int ValorTotal()
        {
            int result = ValorSomenteDoDigito();
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

        protected bool Plural
        {
            get { return _numero > 1; }
        }
    }

    class DigitoUnidade : Digito
    {
        public DigitoUnidade(int numero, int posicao, Digito digitoFilho) : base(numero, posicao, digitoFilho) { }
    }

    class DigitoDezena : Digito
    {
        public DigitoDezena(int numero, int posicao, Digito digitoFilho) : base(numero, posicao, digitoFilho) { }

        public override string Extenso()
        {
            if (_numero == 0)
            {
                return _digitoFilho.Extenso();
            }
            else if (_numero == 1)
            {
                return base.Extenso(ValorSomenteDoDigito() + _digitoFilho.Numero);
            }
            else
            {
                if (_digitoFilho.Numero == 0)
                    return base.Extenso(ValorSomenteDoDigito());
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
        public DigitoCentena(int numero, int posicao, Digito digitoFilho) : base(numero, posicao, digitoFilho) { }

        public override string Extenso()
        {
            if (ValorDosFilhos() == 0)
                return base.Extenso(ValorSomenteDoDigito());
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
