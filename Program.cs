using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.Web.XmlTransform;

namespace Xdt
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 2) Usage("Wrong number of arguments");
            if (!File.Exists(args[0])) Usage("Source file does not exist");
            if (!File.Exists(args[1])) Usage("Transformation file does not exist");
            DoTransform(args[0], args[1]);
        }

        private static void Usage(string errorMessage)
        {
            var err = Console.Error;


            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            err.WriteLine("Xdt Tool " + version);
            err.WriteLine("Applies an XDT transformation to an xml document and writes to standard out");
            err.WriteLine();

            if (errorMessage != null)
            {
                var currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                err.WriteLine("---------- ERROR --------");
                err.WriteLine(errorMessage);
                err.WriteLine();
                Console.ForegroundColor = currentColor;
            }

            err.WriteLine("Usage:");
            err.WriteLine("   xdt <input> <transform>");
            err.WriteLine("   <input> is a file to apply the transformation to");
            err.WriteLine("   <transform> is the xdt transformation file");
            err.WriteLine();
            err.WriteLine("Example");
            err.WriteLine("   xdt Web.config Web.Test.config > result.config");
            err.WriteLine("   copy Web.config Web.config.orig");
            err.WriteLine("   move /Y result.config Web.config");
            Environment.Exit(1);

        }

        private static void DoTransform(string sourceFile, string transformFile)
        {
            using (XmlTransformableDocument document = new XmlTransformableDocument())
            using (XmlTransformation transformation = new XmlTransformation(transformFile))
            {
                document.PreserveWhitespace = true;
                document.Load(sourceFile);
                
                var success = transformation.Apply(document);

                if (success)
                {
                    var settings = new XmlWriterSettings {Encoding = Encoding.UTF8};
                    var writer = XmlWriter.Create(Console.Out, settings);
                    document.WriteTo(writer);
                    writer.Flush();
                }
            }
        }
    }
}
