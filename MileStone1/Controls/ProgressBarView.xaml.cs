﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace MileStone1.Controls
{
    /// <summary>
    /// Interaction logic for 
    /// .xaml
    /// </summary>
    public partial class ProgressBarView : UserControl
    {
        public static readonly DependencyProperty LineToTransmitProperty = DependencyProperty.Register("LineToTransmit", typeof(int), typeof(ProgressBarView));
        public static readonly DependencyProperty RunAnimationProperty = DependencyProperty.Register("RunAnimation", typeof(bool), typeof(ProgressBarView));
        int currentLine;
        ProgressBarVM pbvm;
        int sliderLine;
        //bool animationChanged = false;
        bool runAnimation = true;
        double speed;
        int frameRate = 10;//default value for frame rate is 10
        int secondFromBegining = 0; //second from the begining of the animation
        public int SecondFromBegining
        {
            get
            {
                return secondFromBegining;
            }
            set
            {
                this.secondFromBegining = value;
                timeBlock.Text = new DateTime(2021,4,1,SecondFromBegining / 3600, SecondFromBegining / 60, SecondFromBegining % 60).ToString("HH:mm:ss");
            }


        }
        public int FrameRate {
            get {
                return this.frameRate;
            } set {
                this.frameRate = value;
                Speed = this.Speed; //use the functionality of Speed property
            }
        
        
        }
        public int SliderLine 
        {
            get {return this.sliderLine; }
            set { this.sliderLine = value; }
        }
        public bool RunAnimation
        {
            get { return this.runAnimation; }
            set { this.runAnimation = value;
                this.pbvm.VM_RunAnimation = value;
            }
        }
        public double Speed {
            get { return this.speed; }
            set {
                if ((value <= 2) && (value > 0))
                { //2 is the limit speed
                    this.speed = value;
                    this.pbvm.VM_FramePerSecond = (int)(FrameRate * value);
                }
            }
        }


        public ProgressBarVM Pbvm
            {
            get
            {
                return this.pbvm;
            }
            set
            {
                this.pbvm = value;
                DataContext = pbvm;

                pbvm.PropertyChanged+=delegate(object sender, PropertyChangedEventArgs e)
                {
                    if(e.PropertyName == "VM_LineToTransmitChanged") //change only if the listTransmitor in the model changed
                    {
                        this.Dispatcher.Invoke((Action)(() =>
                            {
                                if (slider.Value != ((double)pbvm.VM_LineToTransmit / pbvm.VM_ListSize) * slider.Maximum)
                                {
                                    slider.Value = ((double)pbvm.VM_LineToTransmit / pbvm.VM_ListSize) * slider.Maximum;
                                }
                            }));
                           
                    }
                    
                };

                pbvm.VM_FramePerSecond = FrameRate; //in the begining the fps is the frame rate (speed is x1)
                //binding propeties
                
            }

}
        public int LineToTransmit
        {
            get
            {
                return this.currentLine;
            }
            set
            {
                
                
                if (this.currentLine != value)
                {
                    this.currentLine = value;
                    SecondFromBegining = (int)(this.currentLine / frameRate);
                    pbvm.VM_LineToTransmit = value;
                }

            }
        }
        
        public ProgressBarView()
        {
            InitializeComponent();
            this.speed = 1;
            //DataContext = vm; //the context is the view model
        }
        

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            System.Windows.Controls.Slider sliderOb = sender as System.Windows.Controls.Slider;
            //double ratio = e.NewValue / sliderOb.ActualWidth; //check what with zero
            double ratio = e.NewValue / sliderOb.Maximum; //check what with zero
            SliderLine = (int)(ratio * pbvm.VM_ListSize);
            LineToTransmit = SliderLine;
            
        }

        private void pauseBottum_Click(object sender, RoutedEventArgs e)
        {
            if (pauseButton.Content == ">")
            {
                pauseButton.Content = "| |";
                RunAnimation = true;
            }
            else {
                pauseButton.Content = ">";
                RunAnimation = false;


            }
        }

        private void fastButton_Click(object sender, RoutedEventArgs e)
        {
            this.Speed = this.Speed + 0.25;
            this.speedText.Text = "SPEED: X" + Speed;
        }

        private void slowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Speed = this.Speed - 0.25;
            this.speedText.Text = "SPEED: X" + Speed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // start the socker to the flighgeart
            if (soketContorller.Content == "Open Socket")
            {
                Pbvm.VM_StartTransimttion();
                soketContorller.Content = "Close Socket";
            }
            else
            {
                Pbvm.VM_StopTransimttion();
                soketContorller.Content = "Open Socket";
            }
        }
    }
}
