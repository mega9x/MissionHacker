using System.Diagnostics;
using System.Net.Http.Json;
using System.Security.Cryptography;
using ConstStr;
using MissionHacker.ConfigHelper;
using MissionHandler.InfoGen;
using Models.Data;
using Models.Enum;
using Models.Photo;
using OutlookHacker.Main.MailName;

namespace Utils.PersonFactory;

public class Person
{
    public static async Task<SinglePerson> GetRandomPerson()
    {
        var randomGen = new RandomGen();
        var date = Date.GenRandomDate();
        
        var person = new SinglePerson()
        {
            Firstname = randomGen.GetFirstName(),
            Lastname = randomGen.GetLastName(),
            Birthmonth = date.Month,
            Birthday = date.Day,
            Nickname = randomGen.GetRandomName(),
            Profession = randomGen.GetRandomProfession(),
        };
        var client = new HttpClient()
        {
            BaseAddress = new(Config.Instance.MissionHackerConfig.PhotoApi.Base)
        };
        var request = new PhotoRequest()
        {
            Age = person.Age switch
            {
                >= 60 => AgeRange.Old,
                >= 30 and <= 45 => AgeRange.MidAged,
                _ => AgeRange.Young,
            },
            Gender = person.Sex switch
            {
                Sex.Female => Gender.Female,
                Sex.Male => Gender.Male,
                _ => throw new ArgumentOutOfRangeException()
            },
            Name = "",
            Num = 6,
        };
        person.Tall = person.Sex == Sex.Male ? RandomNumberGenerator.GetInt32(165, 190) : RandomNumberGenerator.GetInt32(150, 181);
        var response = await client.PostAsJsonAsync(Config.Instance.MissionHackerConfig.PhotoApi.GetPhotoEndpoint, request);
        person.Photos = await response.Content.ReadFromJsonAsync<List<string>>();
        return person;
    }
}