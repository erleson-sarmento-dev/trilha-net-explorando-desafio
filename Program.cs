using System.Globalization;
using System.Text;
using DesafioProjetoHospedagem.Models;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("========================================");
Console.WriteLine("    SISTEMA DE HOSPEDAGEM DE HOTEL      ");
Console.WriteLine("========================================\n");

// 1. Menu para Escolha do Tipo de Suíte
Suite suite = null;

while (suite == null)
{
    Console.WriteLine("--- MENU: Escolha o Tipo de Suíte ---");
    Console.WriteLine("1 - Suíte Básica       | Capacidade: 1 pessoa  | Diária: R$ 50,00");
    Console.WriteLine("2 - Suíte Padrão       | Capacidade: 2 pessoas | Diária: R$ 80,00");
    Console.WriteLine("3 - Suíte Premium      | Capacidade: 4 pessoas | Diária: R$ 150,00");
    Console.WriteLine("4 - Suíte Presidencial | Capacidade: 6 pessoas | Diária: R$ 300,00");
    Console.WriteLine("5 - Outro (Personalizada)");
    Console.Write("Digite o número da opção desejada (1 a 5): ");

    string opcao = Console.ReadLine()?.Trim();

    switch (opcao)
    {
        case "1":
            suite = new Suite(tipoSuite: "Suíte Básica", capacidade: 1, valorDiaria: 50.00m);
            break;
        case "2":
            suite = new Suite(tipoSuite: "Suíte Padrão", capacidade: 2, valorDiaria: 80.00m);
            break;
        case "3":
            suite = new Suite(tipoSuite: "Suíte Premium", capacidade: 4, valorDiaria: 150.00m);
            break;
        case "4":
            suite = new Suite(tipoSuite: "Suíte Presidencial", capacidade: 6, valorDiaria: 300.00m);
            break;
        case "5":
            Console.WriteLine("\n--- Cadastro de Suíte Personalizada ---");
            Console.Write("Digite o nome/tipo da suíte: ");
            string nomePersonalizado = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(nomePersonalizado))
            {
                nomePersonalizado = "Suíte Customizada";
            }

            int capPers;
            while (true)
            {
                Console.Write("Digite a capacidade máxima de pessoas: ");
                if (int.TryParse(Console.ReadLine(), out capPers) && capPers > 0)
                {
                    break;
                }
                Console.WriteLine("Valor inválido! Digite um número inteiro maior que 0.");
            }

            decimal valorPers;
            while (true)
            {
                Console.Write("Digite o valor da diária: R$ ");
                string entradaValor = Console.ReadLine()?.Replace(".", ",");
                if (decimal.TryParse(entradaValor, NumberStyles.Currency, new CultureInfo("pt-BR"), out valorPers) && valorPers > 0)
                {
                    break;
                }
                Console.WriteLine("Valor inválido! Digite um valor monetário positivo.");
            }

            suite = new Suite(tipoSuite: nomePersonalizado, capacidade: capPers, valorDiaria: valorPers);
            break;

        default:
            Console.WriteLine("\nOpção inválida! Por favor, selecione um número de 1 a 5.\n");
            break;
    }
}

Console.WriteLine($"\n>> Suíte Selecionada: {suite.TipoSuite} (Capacidade: {suite.Capacidade} hóspedes | Diária: R$ {suite.ValorDiaria:F2})");

// 2. Criação da Reserva
Console.WriteLine("\n--- Dados da Reserva ---");
int diasReservados;
while (true)
{
    Console.Write("Digite a quantidade de dias reservados: ");
    if (int.TryParse(Console.ReadLine(), out diasReservados) && diasReservados > 0)
    {
        break;
    }
    Console.WriteLine("Valor inválido! Digite um número de dias válido (maior que 0).");
}

Reserva reserva = new Reserva(diasReservados: diasReservados);
reserva.CadastrarSuite(suite);

// 3. Cadastro dos Hóspedes
Console.WriteLine("\n--- Cadastro de Hóspedes ---");
int quantidadeHospedes;
while (true)
{
    Console.Write("Digite a quantidade de hóspedes que deseja cadastrar: ");
    if (int.TryParse(Console.ReadLine(), out quantidadeHospedes) && quantidadeHospedes > 0)
    {
        break;
    }
    Console.WriteLine("Valor inválido! Digite um número maior que 0.");
}

List<Pessoa> hospedes = new List<Pessoa>();
for (int i = 1; i <= quantidadeHospedes; i++)
{
    Console.Write($"Digite o nome completo do hóspede {i}: ");
    string nomeCompleto = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(nomeCompleto))
    {
        nomeCompleto = $"Hóspede {i}";
    }

    string[] partes = nomeCompleto.Split(' ', 2);
    string nome = partes[0];
    string sobrenome = partes.Length > 1 ? partes[1] : "";

    hospedes.Add(new Pessoa(nome: nome, sobrenome: sobrenome));
}

// 4. Efetuar reserva e validar regras
Console.WriteLine("\n========================================");
Console.WriteLine("         RESUMO DA RESERVA             ");
Console.WriteLine("========================================");

try
{
    reserva.CadastrarHospedes(hospedes);

    Console.WriteLine($"Suíte: {suite.TipoSuite} (Capacidade: {suite.Capacidade})");
    Console.WriteLine($"Dias reservados: {reserva.DiasReservados}");
    Console.WriteLine($"Quantidade de hóspedes: {reserva.ObterQuantidadeHospedes()}");
    
    Console.WriteLine("Hóspedes cadastrados:");
    foreach (var hospede in reserva.Hospedes)
    {
        Console.WriteLine($" - {hospede.NomeCompleto}");
    }

    decimal valorTotal = reserva.CalcularValorDiaria();
    if (reserva.DiasReservados >= 10)
    {
        Console.WriteLine("\n* Parabéns! Desconto de 10% aplicado para reservas a partir de 10 dias. *");
    }

    Console.WriteLine($"Valor total da reserva: R$ {valorTotal:F2}");
}
catch (Exception ex)
{
    Console.WriteLine($"\n[ERRO NA RESERVA]: {ex.Message}");
    Console.WriteLine($"Capacidade da suíte ({suite.Capacidade}) é menor que o total de hóspedes ({hospedes.Count}).");
}

Console.WriteLine("========================================\n");