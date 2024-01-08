using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxBOT
{
    internal class JsonReader
    {
        // This class will read and Extract the file
        public string token {  get; set; }
        public string prefix { get; set; }


        public async Task ReadJSON()
        {
            using (StreamReader sr = new StreamReader("config.json")) // to read the json from the File
            {
             string json = await sr.ReadToEndAsync();                                              // Read the file from start to end  & Extract THE DATA
                JsonStructure data = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonStructure>(json);  // To convert from string to object
                this.token = data.token;
                this.prefix = data.prefix;
            }
        }

    }

    /// <summary>
    /// //////////////////////////  TO HIDE THE TOKEN
    /// </summary>
    internal sealed class JsonStructure          
    {
        public string token { get; set; }
        public string prefix { get; set; }
    }
}
