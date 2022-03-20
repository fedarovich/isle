using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isle.Configuration;

public interface IValueRepresentationPolicy
{
    ValueRepresentation GetRepresentationOfType<T>();
}