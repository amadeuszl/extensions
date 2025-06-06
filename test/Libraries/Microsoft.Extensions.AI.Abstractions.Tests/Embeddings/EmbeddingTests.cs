﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using Xunit;

namespace Microsoft.Extensions.AI;

public class EmbeddingTests
{
    [Fact]
    public void Embedding_Ctor_Roundtrips()
    {
        float[] floats = [1f, 2f, 3f];

        AdditionalPropertiesDictionary props = [];
        var createdAt = DateTimeOffset.Parse("2022-01-01T00:00:00Z");
        const string Model = "text-embedding-3-small";

        Embedding<float> e = new(floats)
        {
            CreatedAt = createdAt,
            ModelId = Model,
            AdditionalProperties = props,
        };

        Assert.Equal(floats, e.Vector.ToArray());
        Assert.Equal(Model, e.ModelId);
        Assert.Same(props, e.AdditionalProperties);
        Assert.Equal(createdAt, e.CreatedAt);

        Assert.True(MemoryMarshal.TryGetArray(e.Vector, out ArraySegment<float> array));
        Assert.Same(floats, array.Array);
    }

    [Fact]
    public void Embedding_Byte_SerializationRoundtrips()
    {
        byte[] bytes = [1, 2, 3];
        Embedding<byte> e = new(bytes);

        string json = JsonSerializer.Serialize(e, TestJsonSerializerContext.Default.Embedding);
        Assert.Equal("""{"$type":"uint8","vector":"AQID"}""", json);

        Embedding<byte> result = Assert.IsType<Embedding<byte>>(JsonSerializer.Deserialize(json, TestJsonSerializerContext.Default.Embedding));
        Assert.Equal(e.Vector.ToArray(), result.Vector.ToArray());
    }

    [Fact]
    public void Embedding_SByte_SerializationRoundtrips()
    {
        sbyte[] bytes = [1, 2, 3];
        Embedding<sbyte> e = new(bytes);

        string json = JsonSerializer.Serialize(e, TestJsonSerializerContext.Default.Embedding);
        Assert.Equal("""{"$type":"int8","vector":[1,2,3]}""", json);

        Embedding<sbyte> result = Assert.IsType<Embedding<sbyte>>(JsonSerializer.Deserialize(json, TestJsonSerializerContext.Default.Embedding));
        Assert.Equal(e.Vector.ToArray(), result.Vector.ToArray());
    }

#if NET
    [Fact]
    public void Embedding_Half_SerializationRoundtrips()
    {
        Half[] halfs = [(Half)1f, (Half)2f, (Half)3f];
        Embedding<Half> e = new(halfs);

        string json = JsonSerializer.Serialize(e, TestJsonSerializerContext.Default.Embedding);
        Assert.Equal("""{"$type":"float16","vector":[1,2,3]}""", json);

        Embedding<Half> result = Assert.IsType<Embedding<Half>>(JsonSerializer.Deserialize(json, TestJsonSerializerContext.Default.Embedding));
        Assert.Equal(e.Vector.ToArray(), result.Vector.ToArray());
    }
#endif

    [Fact]
    public void Embedding_Single_SerializationRoundtrips()
    {
        float[] floats = [1f, 2f, 3f];
        Embedding<float> e = new(floats);

        string json = JsonSerializer.Serialize(e, TestJsonSerializerContext.Default.Embedding);
        Assert.Equal("""{"$type":"float32","vector":[1,2,3]}""", json);

        Embedding<float> result = Assert.IsType<Embedding<float>>(JsonSerializer.Deserialize(json, TestJsonSerializerContext.Default.Embedding));
        Assert.Equal(e.Vector.ToArray(), result.Vector.ToArray());
    }

    [Fact]
    public void Embedding_Double_SerializationRoundtrips()
    {
        double[] floats = [1f, 2f, 3f];
        Embedding<double> e = new(floats);

        string json = JsonSerializer.Serialize(e, TestJsonSerializerContext.Default.Embedding);
        Assert.Equal("""{"$type":"float64","vector":[1,2,3]}""", json);

        Embedding<double> result = Assert.IsType<Embedding<double>>(JsonSerializer.Deserialize(json, TestJsonSerializerContext.Default.Embedding));
        Assert.Equal(e.Vector.ToArray(), result.Vector.ToArray());
    }
}
