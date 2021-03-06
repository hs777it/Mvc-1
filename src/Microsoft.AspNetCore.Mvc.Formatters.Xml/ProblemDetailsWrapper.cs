// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Microsoft.AspNetCore.Mvc.Formatters.Xml
{
    /// <summary>
    /// Wrapper class for <see cref="Mvc.ProblemDetails"/> to enable it to be serialized by the xml formatters.
    /// </summary>
    [XmlRoot(nameof(ProblemDetails))]
    public class ProblemDetailsWrapper : IXmlSerializable, IUnwrappable
    {
        /// <summary>
        /// Key used to represent dictionary elements with empty keys
        /// </summary>
        protected static readonly string EmptyKey = SerializableErrorWrapper.EmptyKey;

        /// <summary>
        /// Initializes a new instance of <see cref="ProblemDetailsWrapper"/>.
        /// </summary>
        public ProblemDetailsWrapper()
            : this(new ProblemDetails())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ProblemDetailsWrapper"/>.
        /// </summary>
        public ProblemDetailsWrapper(ProblemDetails problemDetails)
        {
            ProblemDetails = problemDetails;
        }

        internal ProblemDetails ProblemDetails { get; }

        /// <inheritdoc />
        public XmlSchema GetSchema() => null;

        /// <inheritdoc />
        public virtual void ReadXml(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (reader.IsEmptyElement)
            {
                reader.Read();
                return;
            }

            reader.ReadStartElement();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                var key = XmlConvert.DecodeName(reader.LocalName);
                ReadValue(reader, key);

                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        /// <summary>
        /// Reads the value for the specified <paramref name="name"/> from the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/>.</param>
        /// <param name="name">The name of the node.</param>
        protected virtual void ReadValue(XmlReader reader, string name)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var value = reader.ReadInnerXml();

            switch (name)
            {
                case nameof(ProblemDetails.Detail):
                    ProblemDetails.Detail = value;
                    break;

                case nameof(ProblemDetails.Instance):
                    ProblemDetails.Instance = value;
                    break;

                case nameof(ProblemDetails.Status):
                    ProblemDetails.Status = string.IsNullOrEmpty(value) ?
                        (int?)null :
                        int.Parse(value, CultureInfo.InvariantCulture);
                    break;

                case nameof(ProblemDetails.Title):
                    ProblemDetails.Title = value;
                    break;

                case nameof(ProblemDetails.Type):
                    ProblemDetails.Type = value;
                    break;

                default:
                    if (string.Equals(name, EmptyKey, StringComparison.Ordinal))
                    {
                        name = string.Empty;
                    }

                    ProblemDetails.Extensions.Add(name, value);
                    break;
            }
        }

        /// <inheritdoc />
        public virtual void WriteXml(XmlWriter writer)
        {
            if (!string.IsNullOrEmpty(ProblemDetails.Detail))
            {
                writer.WriteElementString(
                    XmlConvert.EncodeLocalName(nameof(ProblemDetails.Detail)),
                    ProblemDetails.Detail);
            }

            if (!string.IsNullOrEmpty(ProblemDetails.Instance))
            {
                writer.WriteElementString(
                    XmlConvert.EncodeLocalName(nameof(ProblemDetails.Instance)),
                    ProblemDetails.Instance);
            }

            if (ProblemDetails.Status.HasValue)
            {
                writer.WriteStartElement(XmlConvert.EncodeLocalName(nameof(ProblemDetails.Status)));
                writer.WriteValue(ProblemDetails.Status.Value);
                writer.WriteEndElement();
            }

            if (!string.IsNullOrEmpty(ProblemDetails.Title))
            {
                writer.WriteElementString(
                    XmlConvert.EncodeLocalName(nameof(ProblemDetails.Title)),
                    ProblemDetails.Title);
            }

            if (!string.IsNullOrEmpty(ProblemDetails.Type))
            {
                writer.WriteElementString(
                    XmlConvert.EncodeLocalName(nameof(ProblemDetails.Type)),
                    ProblemDetails.Type);
            }

            foreach (var keyValuePair in ProblemDetails.Extensions)
            {
                var key = keyValuePair.Key;
                var value = keyValuePair.Value;

                if (string.IsNullOrEmpty(key))
                {
                    key = EmptyKey;
                }

                writer.WriteStartElement(XmlConvert.EncodeLocalName(key));
                if (value != null)
                {
                    writer.WriteValue(value);
                }

                writer.WriteEndElement();
            }
        }

        object IUnwrappable.Unwrap(Type declaredType)
        {
            if (declaredType == null)
            {
                throw new ArgumentNullException(nameof(declaredType));
            }

            return ProblemDetails;
        }
    }
}
