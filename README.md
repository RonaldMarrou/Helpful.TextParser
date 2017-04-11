# Helpful.TextParser [![Build status](https://ci.appveyor.com/api/projects/status/el19jp2ayd9ssg3n?svg=true)](https://ci.appveyor.com/project/RonaldMarrou/helpful-textparser) [![Coverage Status](https://coveralls.io/repos/github/RonaldMarrou/Helpful.TextParser/badge.svg?branch=master)](https://coveralls.io/github/RonaldMarrou/Helpful.TextParser?branch=master)
Helpful.TextParser is a library to convert structured string content into objects.

###Parsing Type

* **Parent/Children Objects**: Occurs when the string content has an structure that works with parent and children objects. Every parent/children line contains a **Tag** that identifies its level.
* **Just Objects**: Occurs when all lines of the string content only belongs to an specific type.

###Structure Types

* **Content Delimited by String**: When each of string content lines are separated by an specific character (e.g. a coma)
* **Positioned**: When each part of the content is located on an specific position of each of string content lines.

###How to Define the Parser Specification

* **Content Delimited By String and Parent/Children Objects**:  Once we specify the Delimitation String for the Parser, you have to Map it to a class, indicating the tag that identifies all data belonging to that level. Also, the position of the tag is required.

Once the Tag is specified, we have to detail the properties we are mapping, indicating their positions and if they are required or not.

In case there is a child class to be mapped, **we have to use a Generic List typed property**. Then, we have to map this property to the class the type of the Generic List belongs to, indicating its tag.

The definition of property mapping and children classes for children works the same way as the first child level.

```csharp
var parser = _fluentParser.Delimited(",").MapTo<DummyFooClass1>("HEADER").Position(0).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(1).Required();
                    properties.Property(x => x.Property2).Position(2).Required();
                    properties.Property(x => x.Property3).Position(3).Required();
                    properties.Property(x => x.Property4).Position(4).Required();
                    properties.Property(x => x.Property5).Position(5).Required();
                    properties.Property(x => x.Property6).Position(6).NotRequired();

                    properties.Property(x => x.Property7).MapTo<DummyFooClass2>("DETAIL").Position(0).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(1).Required();
                            childProperties.Property(x => x.Property2).Position(2).Required();
                            childProperties.Property(x => x.Property3).Position(3).Required();
                            childProperties.Property(x => x.Property4).Position(4).Required();
                            childProperties.Property(x => x.Property5).Position(5).Required();
                            childProperties.Property(x => x.Property6).Position(6).NotRequired();                        });
                });
```

* **Positioned Content and Parent/Children Objects**:  you have to Map it to a class, indicating the tag that identifies all data belonging to that level. Also, the Start/End position of the tag is required.

Once the Tag is specified, we have to detail the properties we are mapping, indicating their positions and if they are required or not.

In case there is a child class to be mapped, **we have to use a Generic List typed property**. Then, we have to map this property to the class the type of the Generic List belongs to, indicating its tag.

The definition of property mapping and children classes for children works the same way as the first child level.

```csharp
            var parser = _fluentParser.Positioned().MapTo<DummyFooClass1>("HH").Position(0, 2).Properties(
                properties =>
                {
                    properties.Property(x => x.Property1).Position(2, 11).Required();
                    properties.Property(x => x.Property2).Position(11, 19).Required();
                    properties.Property(x => x.Property3).Position(19, 26).Required();

                    properties.Property(x => x.Property7).MapTo<DummyFooClass2>("DD").Position(0, 2).Properties(
                        childProperties =>
                        {
                            childProperties.Property(x => x.Property1).Position(2, 11).Required();
                            childProperties.Property(x => x.Property2).Position(11, 19).Required();
                            childProperties.Property(x => x.Property3).Position(19, 26).Required();

                            childProperties.Property(x => x.Property7).MapTo<DummyFooClass3>("SD").Position(0, 2).Properties(
                                grandChildProperties =>
                                {
                                    grandChildProperties.Property(x => x.Property1).Position(2, 11).Required();
                                    grandChildProperties.Property(x => x.Property2).Position(11, 19).Required();
                                    grandChildProperties.Property(x => x.Property3).Position(19, 26).Required();
                                });
                        });
                });
```