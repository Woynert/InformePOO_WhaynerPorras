﻿#pragma checksum "..\..\Game.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "01C86B0FBA7765474C8752CF6FEFA3A77B39F287ACD302F0D27D77380E7295E4"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using BlackJack;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace BlackJack {
    
    
    /// <summary>
    /// Game
    /// </summary>
    public partial class Game : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\Game.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPedir;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\Game.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPlantar;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Game.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle rctTest;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\Game.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle rctTest2;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\Game.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgImage;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\Game.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgImagePlace;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\Game.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblTest2;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\Game.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgTest2;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/BlackJack;component/game.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Game.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btnPedir = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\Game.xaml"
            this.btnPedir.Click += new System.Windows.RoutedEventHandler(this.btnPedir_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnPlantar = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\Game.xaml"
            this.btnPlantar.Click += new System.Windows.RoutedEventHandler(this.btnPlantar_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.rctTest = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 4:
            this.rctTest2 = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 5:
            this.imgImage = ((System.Windows.Controls.Image)(target));
            return;
            case 6:
            this.imgImagePlace = ((System.Windows.Controls.Image)(target));
            return;
            case 7:
            this.lblTest2 = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.imgTest2 = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

