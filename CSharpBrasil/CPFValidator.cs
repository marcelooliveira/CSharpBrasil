using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CSharpBrasil
{
    public class CPFValidator
    {
        public bool IsValid(string cpf)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(cpf))
            {
                string unformattedCPF = cpf;
                CheckUnformattedCPF(unformattedCPF);
                CheckCPFLength(unformattedCPF);
                string trechoCPF = unformattedCPF.Substring(0, 9);

                int digito1 = GetDigitoVerificador(trechoCPF);
                int digito2 = GetDigitoVerificador(trechoCPF + digito1.ToString());

                result = unformattedCPF == trechoCPF + digito1.ToString() + digito2.ToString();
            }
            return result;
        }

        private void CheckUnformattedCPF(string unformattedCPF)
        {
            string unformattedCPFPattern = @"^\d{11}$";
            Regex regex = new Regex(unformattedCPFPattern);
            if (!regex.IsMatch(unformattedCPF))
                throw new CPFInvalidDigits();
        }

        private static void CheckCPFLength(string cpf)
        {
            if (cpf.Length < 11)
                throw new CPFInvalidDigits();
        }

        private int GetDigitoVerificador(string trechoCPF)
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

        private int GetSubtracao(int soma)
        {
            int resto = soma % 11;
            int subtracao = 11 - resto;
            return subtracao;
        }

        private int GetSoma(string trechoCPF, List<int> digitos, List<int> multiplicadores)
        {
            int soma = 0;
            for (int i = 0; i < trechoCPF.Count(); i++)
                soma += digitos[i] * multiplicadores[i];
            return soma;
        }

        private List<int> GetMultiplicadores(List<int> digitos)
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
