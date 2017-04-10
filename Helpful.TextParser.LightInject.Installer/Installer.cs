using Helpful.TextParser.Impl;
using Helpful.TextParser.Impl.LineValueExtractor;
using Helpful.TextParser.Interface;
using LightInject;

namespace Helpful.TextParser.LightInject.Installer
{
    public static class Installer
    {
        public static void InstallTextParser(this IServiceContainer container)
        {
            container.Register<IFluentParser, FluentParser>(typeof(FluentParser).FullName, new PerContainerLifetime());
            container.Register<IParser, Parser>(typeof(Parser).FullName, new PerContainerLifetime());
            container.Register<ILineValueExtractor, DelimitedLineValueExtractor>(typeof(DelimitedLineValueExtractor).FullName, new PerContainerLifetime());
            container.Register<ILineValueExtractor, PositionedLineValueExtractor>(typeof(PositionedLineValueExtractor).FullName, new PerContainerLifetime());
            container.Register<ILineValueExtractorFactory, LineValueExtractorFactory>(new PerContainerLifetime());
            container.Register<IValueSetter, ValueSetter>(new PerContainerLifetime());
        }
    }
}
