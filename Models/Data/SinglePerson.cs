using System.Security.Cryptography;
using Models.Enum;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Models.Data;

public class SinglePerson
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public int Birthyear { get; set; } = RandomNumberGenerator.GetInt32(1957, 1994);
    public int Birthmonth { get; set; }
    public int Birthday { get; set; }
    public Sex Sex { get; set; } = RandomNumberGenerator.GetInt32(0, 2) == 0 ? Sex.Male : Sex.Female;
    public int Tall { get; set; }
    public string Nickname { get; set; }
    public string Profession { get; set; }
    public int Age => DateTime.Now.Year - Birthyear;
    public List<string>? Photos { get; set; } = new();

    public async Task<FileStream> SaveAsJpg(string filename, int index)
    {
        var href = Photos?[index];
        try
        {
            try
            {
                var base64 = href.Replace("data:image/png;base64,", "");
                using var image = await Image.LoadAsync(new MemoryStream(Convert.FromBase64String(base64)));
                image.Mutate(x => x.Resize(image.Width + 200, image.Height + 200));
                await image.SaveAsJpegAsync(filename);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}