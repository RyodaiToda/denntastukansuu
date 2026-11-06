#ifndef __DKA__
#define __DKA__
#include <complex>
#include <iostream>
using namespace std;

class DK{

    public:
        DK();
        DK(int n);
        DK(int n,double coef[],complex<double> z0[]);
        DK(int n,double coef[]);
        DK(int n,double coef[],int ite);
        DK(string filename);
        DK(string file_in, string file_out);
        ~DK();

        double Get_function_value(double t);
        void Calclate_zero_point(int ite);
        void Save_output(string filename);
        void Set_initial_value(int r);

        complex<double> func(complex<double> zi);
        complex<double> dfunc(complex<double> zi);

        void Show_degree();
        void Show_coefficient();
        void Show_z0();
        void Set_file(string file_in);


        static void Calculate_DK(DK dki); 

    private:
        
        int degree;
        complex<double> *z;
        double *coefficient;
        int iteratoion;

    
};


#endif