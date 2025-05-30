using SwipeLab.Domain.DatingProfile.Enums;
using SwipeLab.Domain.Enums;

namespace SwipeLab.Services.Utils;

public static class NameGenerator
{
    private static readonly Random random = new Random();

    private static readonly Dictionary<Ethnicity, Dictionary<Gender, List<string>>> names = new()
{
    {
        Ethnicity.White, new Dictionary<Gender, List<string>>()
        {
            { Gender.Female, new List<string> {
                "Charlotte", "Amelia", "Lily", "Sophie", "Grace", "Emma", "Isla", "Harper", "Abigail", "Ella",
                "Hannah", "Matilda", "Freya", "Scarlett", "Lucy", "Nina", "Alice", "Ruby", "Julia", "Elena",
                "Eva", "Stella", "Lea", "Zoe", "Maya", "Ivy", "Victoria", "Clara", "Anna", "Josephine"
            }},
            { Gender.Male, new List<string> {
                "James", "Henry", "Oliver", "Thomas", "Leo", "Sebastian", "William", "Jack", "Arthur", "Finn",
                "Alexander", "Oscar", "Lucas", "Max", "Noah", "Elias", "Julian", "Nathan", "Frederick", "Liam",
                "Isaac", "Miles", "Samuel", "Louis", "Gabriel", "Theo", "Daniel", "Benjamin", "Elliot", "Hugo"
            }}
        }
    },
    {
        Ethnicity.Asian, new Dictionary<Gender, List<string>>()
        {
            { Gender.Female, new List<string> {
                "Mei", "Hana", "Aisha", "Rina", "Ananya", "Keiko", "Minji", "Yuki", "Saanvi", "Nari",
                "Li", "Sakura", "Priya", "Jiwoo", "Nila", "Siti", "Aya", "Yue", "Tanvi", "Mai",
                "Mina", "Aiko", "Rumi", "Kavya", "Haruka", "Reina", "Yuna", "Linh", "Ami", "Soraya"
            }},
            { Gender.Male, new List<string> {
                "Hiro", "Jin", "Raj", "Takeshi", "Anil", "Soo", "Yuto", "Daisuke", "Minh", "Akira",
                "Kenta", "Arjun", "Sang", "Tariq", "Sunil", "Kenji", "Joon", "Reza", "Ravi", "Daiki",
                "Ayaan", "Yohan", "Ryo", "Hassan", "Farid", "Ali", "Ahmad", "Bin", "Tan", "Zain"
            }}
        }
    },
    {
        Ethnicity.Black, new Dictionary<Gender, List<string>>()
        {
            { Gender.Female, new List<string> {
                "Zuri", "Imani", "Nia", "Ayana", "Asha", "Kamaria", "Makeda", "Ama", "Tia", "Eshe",
                "Monique", "Kiana", "Kenya", "Tanesha", "Aaliyah", "Chiamaka", "Yetunde", "Onika", "Fatou", "Amara",
                "Lulu", "Serena", "Adanna", "Melina", "Thandi", "Lina", "Zahara", "Mireille", "Sade", "Jalia"
            }},
            { Gender.Male, new List<string> {
                "Malik", "Kwame", "Jabari", "Omari", "Sekou", "Jelani", "Dante", "Kofi", "Amari", "Obasi",
                "Tariq", "Jalen", "Akil", "Ayodele", "Darnell", "Kendrick", "Elijah", "Tyrese", "Nasir", "Makonnen",
                "Zuberi", "Taye", "Idris", "Ezekiel", "Isaiah", "Lamine", "Nelson", "Kobe", "Eli", "Musa"
            }}
        }
    },
    {
        Ethnicity.Latino, new Dictionary<Gender, List<string>>()
        {
            { Gender.Female, new List<string> {
                "Camila", "Isabella", "Lucía", "Sofía", "Mariana", "Valentina", "Gabriela", "Ximena", "Ana", "Paloma",
                "Esmeralda", "Lola", "Catalina", "Juana", "Carmen", "Elena", "Yaretzi", "Fernanda", "Dulce", "Rosa",
                "Clara", "Marta", "Beatriz", "Alma", "Carla", "Violeta", "Inés", "Daniela", "Renata", "Nayeli"
            }},
            { Gender.Male, new List<string> {
                "Mateo", "Santiago", "Diego", "Alejandro", "Andrés", "Luis", "Carlos", "Emiliano", "Javier", "Marco",
                "Ramiro", "Rafael", "Hugo", "Felipe", "Esteban", "César", "Gabriel", "Iván", "Pablo", "Manuel",
                "Tomas", "Lucas", "Francisco", "Ernesto", "Bruno", "Alonso", "Adrián", "Benjamín", "Raúl", "Eduardo"
            }}
        }
    }
};

    public static string GetRandomName(Gender gender, Ethnicity ethnicity)
    {
        if (names.TryGetValue(ethnicity, out var genderNames) &&
            genderNames.TryGetValue(gender, out var nameList))
        {
            return nameList[random.Next(nameList.Count)];
        }

        // Fallback to white Danish names if not found
        var fallbackList = names[Ethnicity.White][gender];
        return fallbackList[random.Next(fallbackList.Count)];
    }
}