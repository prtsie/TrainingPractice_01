decimal gold;
int toBuy;
const decimal price = 10;
Console.WriteLine($"Цена кристалла: {price}");

while (true)
{
    Console.Write("Введите ваше золото >>> ");
    if (decimal.TryParse(Console.ReadLine(), out gold) && gold >= 0)
    {
        break;
    }

    Console.WriteLine("Вы ввели невалидное число");
}

while (true)
{
    Console.Write("Введите количество кристаллов для покупки >>> ");
    if (int.TryParse(Console.ReadLine(), out toBuy) && toBuy >= 0)
    {
        break;
    }

    Console.WriteLine("Вы ввели невалидное число");
}

var crystals = 0;
var bruh = gold < toBuy * price || Buy();
Console.WriteLine($"Ваше золото: {gold}");
Console.WriteLine($"Ваши кристаллы: {crystals}");
return;

bool Buy()
{
    crystals = toBuy;
    gold -= crystals * price;
    return true;
}
