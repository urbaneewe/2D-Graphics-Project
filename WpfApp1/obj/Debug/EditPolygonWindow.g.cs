﻿#pragma checksum "..\..\EditPolygonWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "06942F20307C7062DED827028D1F0C5E8D029EB6AC810B6D243A42B00071B422"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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
using WpfApp1;


namespace WpfApp1 {
    
    
    /// <summary>
    /// EditPolygonWindow
    /// </summary>
    public partial class EditPolygonWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\EditPolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox polygonStrokeThickness;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\EditPolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox polygonColors;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\EditPolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox polygonStrokeColors;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\EditPolygonWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button submitPolygon;
        
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
            System.Uri resourceLocater = new System.Uri("/WpfApp1;component/editpolygonwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\EditPolygonWindow.xaml"
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
            this.polygonStrokeThickness = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.polygonColors = ((System.Windows.Controls.ComboBox)(target));
            
            #line 24 "..\..\EditPolygonWindow.xaml"
            this.polygonColors.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.polygonColors_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.polygonStrokeColors = ((System.Windows.Controls.ComboBox)(target));
            
            #line 26 "..\..\EditPolygonWindow.xaml"
            this.polygonStrokeColors.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.polygonStrokeColors_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.submitPolygon = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\EditPolygonWindow.xaml"
            this.submitPolygon.Click += new System.Windows.RoutedEventHandler(this.submitPolygon_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

