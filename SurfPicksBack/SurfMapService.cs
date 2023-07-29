using Microsoft.VisualBasic.FileIO;
using SurfPicksBack.Models;

namespace SurfPicksBack
{
    public class SurfMapService
    {
        private readonly string mapsTablesSorce = "Assets/MapsTables";
        private List<SurfMapDto> allMaps = new List<SurfMapDto>();
        private AvailablePredicatesDto availablePredicates = new AvailablePredicatesDto();

        public SurfMapService()
        {
            InitMaps();
        }

        public AvailablePredicatesDto GetAvailablePredicates()
        {
            return availablePredicates;
        }
        public void InitMaps()
        {
            string[] mapsTablesFiles = Directory.GetFiles(mapsTablesSorce);
            allMaps = new List<SurfMapDto>();
            availablePredicates.Pools = new List<string>();
            availablePredicates.Styles = new List<List<string>>();
            availablePredicates.Tiers = new List<List<string>>();
            foreach (string mapsTableFile in mapsTablesFiles)
            {
                try
                {
                    using (TextFieldParser parser = new TextFieldParser(mapsTableFile))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        while (!parser.EndOfData)
                        {
                            string[] fields = parser.ReadFields();
                            if (fields!=null&&fields.Count()==4)
                            {
                            allMaps.Add(new SurfMapDto()
                                {
                                    Name = fields[1],
                                    Tier = fields[2],
                                    Style = fields[3],
                                    ImageSrc = fields[1] + ".jpg",
                                    Status = SurfMapStatus.None,
                                    Pool = fields[0]
                                });
                                if (!availablePredicates.Pools.Contains(fields[0]) && fields[0]!="")
                                {
                                    availablePredicates.Pools.Add(fields[0]);
                                    availablePredicates.Styles.Add(new List<string>());
                                    availablePredicates.Tiers.Add(new List<string>());
                                }
                                if (!availablePredicates.Tiers[availablePredicates.Pools.FindIndex(x => x == fields[0])].Contains(fields[2]))
                                {
                                    availablePredicates.Tiers[availablePredicates.Pools.FindIndex(x => x == fields[0])].Add(fields[2]);
                                }
                                if (!availablePredicates.Styles[availablePredicates.Pools.FindIndex(x => x == fields[0])].Contains(fields[3]))
                                {
                                    availablePredicates.Styles[availablePredicates.Pools.FindIndex(x => x == fields[0])].Add(fields[3]);
                                }
                            }
                            
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Bad File");
                }
            }
        }

        public List<SurfMapDto> GetSurfMaps(Func<SurfMapDto, bool> predicate, int countOfMaps)
        {
            List<SurfMapDto> surfMaps = allMaps.Where(predicate).ToList();
            Random random = new Random();
            List<int> randomList = new List<int>();
            List<SurfMapDto> pickedMaps = new List<SurfMapDto>();
            if (surfMaps.Count <= countOfMaps)
            {
                return new List<SurfMapDto>();
            }
            int numOfRetries = 10;
            int number = 0;
            for (int i = 0; i < countOfMaps; i++)
            {
                number = random.Next(0, surfMaps.Count);
                if (!randomList.Contains(number))
                {
                    randomList.Add(number);
                    pickedMaps.Add(new SurfMapDto() { Name = surfMaps[number].Name, ImageSrc = surfMaps[number].ImageSrc, Pool = surfMaps[number].Pool,
                        Status = surfMaps[number].Status, Tier = surfMaps[number].Tier, Style = surfMaps[number].Style });
                }
                else
                {
                    numOfRetries--;
                    i--;
                    if (numOfRetries == 0)
                        break;
                }
            }
            return pickedMaps;
        }
    }
}
