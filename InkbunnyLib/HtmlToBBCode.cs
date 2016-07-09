using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib {
    /// <summary>
    /// A modification of HtmlToText in the HTML Agility Pack's examples folder that tries to convert Weasyl's HTML output to Inkbunny's BBCode system.
    /// </summary>
    public static class HtmlToBBCode {
        #region Public Methods

        public static string Convert(string path) {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(path);

            StringWriter sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        public static string ConvertHtml(string html) {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            StringWriter sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        public static void ConvertTo(HtmlNode node, TextWriter outText) {
            string html;
            switch (node.NodeType) {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = ((HtmlTextNode)node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0) {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name) {
                        case "p":
                        case "br":
                            // treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;
                        case "h1":
                        case "h2":
                        case "h3":
                        case "h4":
                        case "h5":
                        case "h6":
                            outText.Write("\r\n[b]");
                            break;
                        case "b":
                        case "strong":
                            outText.Write("[b]");
                            break;
                        case "i":
                        case "em":
                            outText.Write("[i]");
                            break;
                        case "u":
                            outText.Write("[u]");
                            break;
                        case "a":
                            outText.Write("[url=" + node.Attributes["href"].Value + "]");
                            break;
                        case "blockquote":
                            outText.Write("[q]");
                            break;
                    }

                    if (node.HasChildNodes) {
                        ConvertContentTo(node, outText);
                    }

                    switch (node.Name) {
                        case "h1":
                        case "h2":
                        case "h3":
                        case "h4":
                        case "h5":
                        case "h6":
                        case "b":
                        case "strong":
                            outText.Write("[/b]");
                            break;
                        case "i":
                        case "em":
                            outText.Write("[/i]");
                            break;
                        case "u":
                            outText.Write("[/u]");
                            break;
                        case "a":
                            outText.Write("[/url]");
                            break;
                        case "blockquote":
                            outText.Write("[/q]");
                            break;
                    }
                    break;
            }
        }

        #endregion

        #region Private Methods

        private static void ConvertContentTo(HtmlNode node, TextWriter outText) {
            foreach (HtmlNode subnode in node.ChildNodes) {
                ConvertTo(subnode, outText);
            }
        }

        #endregion
    }
}
