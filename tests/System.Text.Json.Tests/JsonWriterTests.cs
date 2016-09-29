﻿using System.IO;
using System.Text.Json;
using System.Text;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class CompositeFormattingTests
    {
        [Fact]
        public void WriteJsonUtf8()
        {
            var formatter = new BufferFormatter(1024, EncodingData.InvariantUtf8);            
            var json = new JsonWriter<BufferFormatter>(formatter, prettyPrint: true);
            Write(ref json);

            var formatted = formatter.Formatted;
            var str = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace("\n", "").Replace(" ", ""));
        }

        [Fact]
        public void WriteJsonUtf16()
        {
            var formatter = new BufferFormatter(1024, EncodingData.InvariantUtf16);
            var json = new JsonWriter<BufferFormatter>(formatter, prettyPrint: false);
            Write(ref json);

            var formatted = formatter.Formatted;
            var str = Encoding.Unicode.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace(" ", ""));
        }

        static string expected = "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052}}";
        static void Write(ref JsonWriter<BufferFormatter> json)
        {
            json.WriteObjectStart();
            json.WriteAttribute("age", 30);
            json.WriteAttribute("first", "John");
            json.WriteAttribute("last", "Smith");
            json.WriteMember("phoneNumbers");
            json.WriteArrayStart();
            json.WriteString("425-000-1212");
            json.WriteString("425-000-1213");
            json.WriteArrayEnd();
            json.WriteMember("address");
            json.WriteObjectStart();
            json.WriteAttribute("street", "1 Microsoft Way");
            json.WriteAttribute("city", "Redmond");
            json.WriteAttribute("zip", 98052);
            json.WriteObjectEnd();
            json.WriteObjectEnd();
        }
    }
}
