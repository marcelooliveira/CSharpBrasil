using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBrasil
{
    public class CPF
    {
        public static bool IsValid(string cpf)
        {
            string trechoCPF = cpf.Substring(0, 9);

            int digito1 = GetDigitoVerificador(trechoCPF);
            int digito2 = GetDigitoVerificador(trechoCPF + digito1.ToString());

            return cpf == trechoCPF + digito1.ToString() + digito2.ToString();
        }

        private static int GetDigitoVerificador(string trechoCPF)
        {
            int result = 0;
            List<int> digitos = GetDigitos(trechoCPF);
            int soma = GetSoma(trechoCPF, digitos, GetMultiplicadores(digitos));
            int subtracao = GetSubtracao(soma);

            if (subtracao > 9)
                result = 0;
            else
                result = subtracao;
            return result;
        }

        private static int GetSubtracao(int soma)
        {
            int resto = soma % 11;
            int subtracao = 11 - resto;
            return subtracao;
        }

        private static int GetSoma(string trechoCPF, List<int> digitos, List<int> multiplicadores)
        {
            int soma = 0;
            for (int i = 0; i < trechoCPF.Count(); i++)
                soma += digitos[i] * multiplicadores[i];
            return soma;
        }

        private static List<int> GetMultiplicadores(List<int> digitos)
        {
            return Enumerable
                .Range(2, digitos.Count())
                .OrderByDescending(m => m)
                .ToList();
        }

        private static List<int> GetDigitos(string trechoCPF)
        {
            return trechoCPF
                .ToCharArray()
                .Select(c => int.Parse(c.ToString()))
                .ToList();
        }
    }
}
