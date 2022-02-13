using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Models
{
    public class ResultWithComparison<T> : IEquatable<ResultWithComparison<T>>
    {
        [JsonPropertyName("courant")] public T current { get; set; }
        public T compare { get; set; }
        public bool Equals(ResultWithComparison<T> other) => GetHashCode() == other.GetHashCode();
        public override bool Equals(object other) => Equals(other as ResultWithComparison<T>);
        public override int GetHashCode() => current?.GetHashCode()??0;

    }
    public class ResultArrayTemplateWithComparison<T>
    {
        public bool success { get; set; }
        public IList<ResultWithComparison<T>> results { get; set; }
    }
    public class ResultTemplate<T>
    {
        public bool success { get; set; }
        public T results { get; set; }
    }
    public class ResultArrayTemplate<T>
    {
        public bool success { get; set; }
        public IList<T> results { get; set; }
    }


}
