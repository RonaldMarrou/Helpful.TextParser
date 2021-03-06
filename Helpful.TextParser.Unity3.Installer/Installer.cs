﻿using Helpful.TextParser.Impl;
using Helpful.TextParser.Impl.LineValueExtractor;
using Helpful.TextParser.Interface;
using Microsoft.Practices.Unity;

namespace Helpful.TextParser.Unity3.Installer
{
    public static class Installer
    {
        public static void InstallTextParser(this UnityContainer container)
        {
            container.RegisterType<IFluentParser, FluentParser>(new ContainerControlledLifetimeManager());
            container.RegisterType<IParser, Parser>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILineValueExtractor, DelimitedLineValueExtractor>(typeof(DelimitedLineValueExtractor).FullName, new ContainerControlledLifetimeManager());
            container.RegisterType<ILineValueExtractor, PositionedLineValueExtractor>(typeof(PositionedLineValueExtractor).FullName, new ContainerControlledLifetimeManager());
            container.RegisterType<ILineValueExtractorFactory, LineValueExtractorFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IValueSetter, ValueSetter>(new ContainerControlledLifetimeManager());
        }
    }
}
