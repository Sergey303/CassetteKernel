using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CassetteKernel
{
    public delegate void LogLine(string message);
    public class CassettesConnection
    {
        public static Dictionary<string, factograph.CassetteInfo> cassettesInfo = new Dictionary<string, factograph.CassetteInfo>();
        private static Dictionary<string, factograph.RDFDocumentInfo> docsInfo = new Dictionary<string, factograph.RDFDocumentInfo>();
        public static void ConnectToCassettes(IEnumerable<XElement> LoadCassette_elements, LogLine protocol)
        {
            cassettesInfo = new Dictionary<string, factograph.CassetteInfo>();
            docsInfo = new Dictionary<string, factograph.RDFDocumentInfo>();

            foreach (XElement lc in LoadCassette_elements)
            {
                bool loaddata = true;
                if (lc.Attribute("regime") != null && lc.Attribute("regime").Value == "nodata") loaddata = false;
                string cassettePath = lc.Value;

                factograph.CassetteInfo ci = null;
                try
                {
                    ci = factograph.Cassette.LoadCassette(cassettePath, loaddata);
                }
                catch (Exception ex)
                {
                    protocol("Ошибка при загрузке кассеты [" + cassettePath + "]: " + ex.Message);
                }
                if (ci == null || cassettesInfo.ContainsKey(ci.fullName)) continue;
                cassettesInfo.Add(ci.fullName.ToLower(), ci);
                if (ci.docsInfo != null) foreach (var docInfo in ci.docsInfo)
                    {
                        docsInfo.Add(docInfo.dbId, docInfo);
                        if (loaddata)
                        {
                            try
                            {
                                docInfo.isEditable = (lc.Attribute("write") != null && docInfo.GetRoot().Attribute("counter") != null);
                                //sDataModel.LoadRDF(docInfo.Root);
                                //if (!docInfo.isEditable) docInfo.root = null; //Иногда это действие нужно закомментаривать...
                            }
                            catch (Exception ex)
                            {
                                protocol("error in document " + docInfo.uri + "\n" + ex.Message);
                            }
                        }
                    }
            }
        }
        public static IEnumerable<factograph.RDFDocumentInfo> GetFogFiles()
        {
            foreach (var cpair in cassettesInfo)
            {
                var ci = cpair.Value;
                var toload = ci.loaddata;
                factograph.RDFDocumentInfo di0 = new factograph.RDFDocumentInfo(ci.cassette, true);
                yield return di0;
                var qu = di0.GetRoot().Elements("document").Where(doc => doc.Element("iisstore").Attribute("documenttype").Value == "application/fog");
                foreach (var docnode in qu)
                {
                    var di = new factograph.RDFDocumentInfo(docnode, ci.cassette.Dir.FullName, toload);
                    if (toload) di.ClearRoot();
                    yield return di;
                }
                di0.ClearRoot();
            }
        }
    }
}
