using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Domain.Abstractions;

namespace ApiKnowledgePortal.Domain.ValueObjects
{
    public sealed class ApiSpecId : ValueObject, IEquatable<ApiSpecId>
    {
        public Guid Value { get; }

        public ApiSpecId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("апи спецификация не может быть пустой", nameof(value));

            Value = value;
        }

        public static ApiSpecId NewId() => new ApiSpecId(Guid.NewGuid());

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }

        public bool Equals(ApiSpecId? other)
        {
            if (other is null) return false;
            return Value.Equals(other.Value);
        }

        public override string ToString() => Value.ToString();

        public static bool operator ==(ApiSpecId left, ApiSpecId right) => Equals(left, right);
        public static bool operator !=(ApiSpecId left, ApiSpecId right) => !Equals(left, right);
    }
}