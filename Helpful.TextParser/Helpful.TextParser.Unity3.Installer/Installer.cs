using Helpful.TextParser.Impl;
using Helpful.TextParser.Impl.LineValueExtractor;
using Helpful.TextParser.Interface;
using Microsoft.Practices.Unity;

namespace Helpful.TextParser.Unity3.Installer
{
    public static class Installer
    {
        public static void InstallTextParser(this UnityContainer container)
        {
            container.RegisterType<IFluentParser, FluentParser>(new TransientLifetimeManager());
            container.RegisterType<IParser, Parser>(new TransientLifetimeManager());
            container.RegisterType<ILineValueExtractor, DelimitedLineValueExtractor>(typeof(DelimitedLineValueExtractor).FullName, new TransientLifetimeManager());
            container.RegisterType<ILineValueExtractor, PositionedLineValueExtractor>(typeof(PositionedLineValueExtractor).FullName, new TransientLifetimeManager());
            container.RegisterType<ILineValueExtractorFactory, LineValueExtractorFactory>(new TransientLifetimeManager());
            container.RegisterType<IValueSetter, ValueSetter>(new TransientLifetimeManager());
        }
    }
}
