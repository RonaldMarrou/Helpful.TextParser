using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Helpful.TextParser.Impl;
using Helpful.TextParser.Impl.LineValueExtractor;
using Helpful.TextParser.Interface;

namespace Helpful.TextParser.Windsor.Installer
{
    public static class Installer
    {
        public static void InstallTextParser(this WindsorContainer container)
        {
            container.Register(
                Component.For<IFluentParser>().ImplementedBy<FluentParser>().Named(typeof(FluentParser).Name).LifestyleSingleton(),
                Component.For<IParser>().ImplementedBy<Parser>().Named(typeof(Parser).Name).LifestyleSingleton(),
                Component.For<ILineValueExtractor>().ImplementedBy<DelimitedLineValueExtractor>().Named(typeof(DelimitedLineValueExtractor).Name).LifestyleSingleton(),
                Component.For<ILineValueExtractor>().ImplementedBy<PositionedLineValueExtractor>().Named(typeof(PositionedLineValueExtractor).Name).LifestyleSingleton(),
                Component.For<ILineValueExtractorFactory>().ImplementedBy<LineValueExtractorFactory>().Named(typeof(LineValueExtractorFactory).Name).LifestyleSingleton(),
                Component.For<IValueSetter>().ImplementedBy<ValueSetter>().Named(typeof(ValueSetter).Name).LifestyleSingleton());
        }
    }
}
