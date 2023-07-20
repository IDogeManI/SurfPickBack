using Microsoft.VisualBasic.FileIO;
using SurfPicksBack.Models;

namespace SurfPicksBack
{
    public class SurfMapService
    {
        private readonly string mapsTablesSorce = "Assets/MapsTables";
        private List<SurfMapDto> allMaps = new List<SurfMapDto>();

        public SurfMapService()
        {
            InitMaps();
        }


        public void InitMaps()
        {
            string[] mapsTablesFiles = Directory.GetFiles(mapsTablesSorce);
            foreach (string mapsTableFile in mapsTablesFiles)
            {
                using (TextFieldParser parser = new TextFieldParser(mapsTableFile))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        int tier;
                        if (int.TryParse(fields[1], out tier))
                        {
                            allMaps.Add(new SurfMapDto()
                            {
                                Name = fields[0],
                                Tier = tier,
                                ImageSrc = "../assets/images/" + fields[0].ToString() + ".jpg",
                                Status = SurfMapStatus.None,
                                Server = mapsTableFile.Contains("Cybershoke") ? "Cybershoke" : mapsTableFile.Contains("KSF") ? "KSF" : ""
                            });
                        }
                    }
                }
            }
        }

        public List<SurfMapDto> GetSurfMaps(Func<SurfMapDto, bool> predicate, int countOfMaps)
        {
            List<SurfMapDto> surfMaps = allMaps.Where(predicate).ToList();
            Random random = new Random();
            List<int> randomList = new List<int>();
            List<SurfMapDto> pickedMaps = new List<SurfMapDto>();
            int number = 0;
            for (int i = 0; i < countOfMaps; i++)
            {
                number = random.Next(0, surfMaps.Count);
                if (!randomList.Contains(number))
                {
                    randomList.Add(number);
                    pickedMaps.Add(new SurfMapDto() { Name = surfMaps[number].Name, ImageSrc = surfMaps[number].ImageSrc, Server = surfMaps[number].Server, Status = surfMaps[number].Status, Tier = surfMaps[number].Tier });
                }
                else
                {
                    i--;
                }
            }
            return pickedMaps;
        }
    }
}
