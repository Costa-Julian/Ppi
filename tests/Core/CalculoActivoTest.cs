using Application;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Core
{
    public class CalculoActivoTest
    {
        private CalculoAccionHandler _accionHandler = null!;
        private CalculoBonoHandler _bonoHandler = null!;
        private CalculoFciHandler _fciHandler = null!;

        [SetUp]
        public void Setup()
        {
            _accionHandler = new CalculoAccionHandler();
            _bonoHandler = new CalculoBonoHandler();
            _fciHandler = new CalculoFciHandler();
        }

        [Test]
        public void CalculoTotal_FCI_MismoPrecioYCantidad_SinComisionNiIva()
        {
            const decimal precio = 10000m;
            const int cantidad = 10;
            var fci = new Fci { PrecioUnitario = precio };

            var esperado = precio * cantidad;

            var total = _fciHandler.CalculoTotal(fci, cantidad);

            total.Should().Be(esperado);
        }

        [Test]
        public void CalculoTotal_Bono_MismoPrecioYCantidad_AplicaComision002YIva21()
        {
            const decimal precio = 10000m;
            const int cantidad = 10;
            var bono = new Bono { PrecioUnitario = precio };

            var bruto = precio * cantidad;                
            var comision = bruto * 0.002m;                 
            var iva = comision * 0.21m;                    
            var esperado = bruto + comision + iva;        

            var total = _bonoHandler.CalculoTotal(bono, cantidad);
            total.Should().Be(esperado);
        }

        [Test]
        public void CalculoTotal_Accion_MismoPrecioYCantidad_AplicaComision006YIva21()
        {
            const decimal precio = 10000m;
            const int cantidad = 10;
            var accion = new Accion { PrecioUnitario = precio };

            var bruto = precio * cantidad;                 
            var comision = bruto * 0.006m;                 
            var iva = comision * 0.21m;                    
            var esperado = bruto + comision + iva;         

            var total = _accionHandler.CalculoTotal(accion, cantidad);
            total.Should().Be(esperado);
        }
    }
}
