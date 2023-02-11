using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using ConstStr;
using HqGpt;
using Models.Data;
using Models.Enum;
using UnitsNet;
using Utils.InfoGen;
using Models.Data;

namespace DataLibs.InfoGen
{
    public static class PersonFactory
    {
        public static List<USAddressModel> USAddress = JsonSerializer.Deserialize<IEnumerable<USAddressModel>>(File.ReadAllText(ConfigPath.US_ADDRESS_DATA_PATH)).ToList();
        public static USAddressModel GetRandomUSAddress()
        {
            return USAddress[RandomNumberGenerator.GetInt32(0, USAddress.Count)];
        }

        public static async Task<SinglePerson> GenPerson()
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
                TextGenerator = new Gpt(Config.Config.Instance.MissionHackerConfig.AiApi.Base),
            };
            var client = new HttpClient()
            {
                BaseAddress = new Uri(Config.Config.Instance.MissionHackerConfig!.PhotoApi.Base)
            };
            person.Tall = person.Sex == Sex.Male ? Length.FromCentimeters(RandomNumberGenerator.GetInt32(165, 190)) : Length.FromCentimeters(RandomNumberGenerator.GetInt32(150, 181));
            var response = await client.PostAsJsonAsync(Config.Config.Instance.MissionHackerConfig!.PhotoApi.GetPhotoEndpoint, person.PhotoRequest);
            var photos = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            person.Photos = photos.ToList();
            return person;
        }
        
        public static async Task<SinglePerson> GenTextPerson()
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
                TextGenerator = new Gpt(Config.Config.Instance.MissionHackerConfig.AiApi.Base),
            };
            var client = new HttpClient()
            {
                BaseAddress = new Uri(Config.Config.Instance.MissionHackerConfig!.PhotoApi.Base)
            };
            person.Tall = person.Sex == Sex.Male ? Length.FromCentimeters(RandomNumberGenerator.GetInt32(165, 190)) : Length.FromCentimeters(RandomNumberGenerator.GetInt32(150, 181));
            var response = await client.PostAsJsonAsync(Config.Config.Instance.MissionHackerConfig!.PhotoApi.GetPhotoEndpoint, person.PhotoRequest);
            var photos = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            person.Photos = photos.ToList();
            return person;
        }
    }
}
