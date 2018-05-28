using System.Net.Http;
using System.Text;

namespace TestZen.Owin.WebApi.Testing
{
    public class BodyContent : StringContent
    {
        /// <summary>
        /// Sets encoding=UTF8 and mediaType=application/json 
        /// </summary>
        /// <param name="content"></param>
        public BodyContent(string content) : base(content, UnicodeEncoding.UTF8, "application/json") { }

        /// <summary>
        /// Sets content to empty, encoding=UTF8 and mediaType=application/json 
        /// </summary>
        /// <param name="content"></param>
        public BodyContent() : base(string.Empty, UnicodeEncoding.UTF8, "application/json") { }
    }
}
