#include <iostream>
#include <complex>
#include "DKA.hpp"
#include <math.h>
#include <iomanip>
#include <fstream>
#include <string>
using namespace std;
const double M_PI = 3.141592653589793;


DK::DK(){

}

DK::DK(int n){
    degree = n;
}

DK::DK(int n, double coef[], complex<double> z0[]){

    degree = n;
    iteratoion = 50;

    z = new complex<double>[degree];
    coefficient = new double[degree+1];

    for(int i=0;i<degree;i++){
        coefficient[i] = coef[i];
        z[i] = z0[i];
    }

    coefficient[degree] = coef[degree];

}

DK::DK(int n, double coef[]){

    degree = n;

    z = new complex<double>[degree];
    coefficient = new double[degree+1];

    for(int i=0;i<=degree;i++){
        coefficient[i] = coef[i];
    }



}

DK::DK(int n, double coef[],int ite){

    degree = n;
    iteratoion = ite;

    z = new complex<double>[degree];

    for(int i=0;i<degree;i++){
        z[i] = coef[i];
    }

}

DK::DK(string filename){

    Set_file(filename);
    //Show_coefficient();

}

DK::DK(string file_in, string file_out){
    Set_file(file_in);
    Set_initial_value(1);
    Calclate_zero_point(100);
    Save_output(file_out);
}

DK::~DK(){
    delete[] z;
    delete[] coefficient;
}

void DK::Calclate_zero_point(int ite){

    int k=0;

    cout<<setw(3)<<k<<",";
    for(int i=0;i<degree;i++){
        cout<<right<<setw(4)<<"z["<<i<<"]="<<right<<setw(23)<<z[i]<<",";
    }

    cout<<endl;

    while(k <= ite){

        for(int i=0;i<degree;i++){
	
			z[i] = z[i]-func(z[i])/dfunc(z[i]);
		}
        k++;
       /*cout<<setw(3)<<k<<",";
		for(int i=0;i<degree;i++){
			cout<<right<<setw(4)<<"z["<<i<<"]="<<right<<setw(23)<<z[i]<<",";
		}
        cout<<endl;
                */ 
    }

    cout<<setw(3)<<k<<",";
    for(int i=0;i<degree;i++){
        cout<<right<<setw(4)<<"z["<<i<<"]="<<right<<setw(23)<<z[i]<<",";
    }

    cout<<endl;
}

complex<double> DK::func(complex<double> zi){

    complex<double> zout(0.0, 0.0);

    for(int i=0;i<=degree;i++){

        zout += pow(zi,i)*coefficient[i];

    }

    return zout;

}

complex<double> DK::dfunc(complex<double> zi){

    complex<double> sum(coefficient[degree], 0.0);

    for(int i=0;i<degree;i++){
		if(zi != z[i])sum *= (zi-z[i]);
	}

    return sum;

}


void DK::Show_degree(){
    std::cout<<"degree = "<<degree<<std::endl;
}

void DK::Show_coefficient(){
    cout<<"coefficient = [ ";

    for(int i=0;i<=degree;i++){
        std::cout<<coefficient[i]<<", ";   
    }

    cout<<"]"<<endl;
}

void DK::Show_z0(){

    cout<<"z0 = [ ";

    for(int i=0;i<degree;i++){
        std::cout<<z[i]<<", ";   
    }

    cout<<"]"<<endl;
}

double DK::Get_function_value(double t){


    return 0.0;
}

void DK::Set_initial_value(int r=5){
    double beta = -1.0 * coefficient[degree - 1] / (degree * coefficient[degree]);
    complex<double> temp;
    for(int i=0;i<degree;i++){
        
        temp = complex<double>(0.0, (2.0 * M_PI * i + 1.5) / degree);
        z[i] = beta + (complex<double>)r*exp(temp);
        //cout<<"beta : "<<beta<<endl;
    }
}

void DK::Set_file(string file_in){
    ifstream ifs(file_in);

    // 開けなかったらエラー
    if( !ifs )
    {
        cout << "Error! File can not be opened." << endl;
        return ;
    }

    string line;

    int i=0;

    while(getline(ifs,line)){
        //cout<<line<<endl;
        i++;
    }

    degree = i-1;
    cout<<"degree"<<degree<<endl;

    z = new complex<double>[degree];
    coefficient = new double[degree + 1];

    ifs.clear();
    ifs.seekg(0,ios::beg);

    i=0;

    while(getline(ifs,line)){
        cout<<line<<endl;
        coefficient[i] = stod(line);
        i++;
    }

    ifs.close();
}


void DK::Save_output(string filename){

    ofstream outputfile(filename);

    for(int i=0;i<degree;i++){
        outputfile<<z[i]<<endl;
    }

    outputfile.close();



}



