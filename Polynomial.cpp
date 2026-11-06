#include "Polynomial.hpp"
#include <iostream>
using namespace std;

Polynomial::Polynomial(int n){
    degree = n;
}

Polynomial::Polynomial(int n, double coef[]){
    degree = n;

    coefficient = new double[degree];

    for(int i=0;i<n;i++){
        coefficient[i] = coef[i];
    }

}

Polynomial::Polynomial(){

}

Polynomial::~Polynomial(){

    delete[] coefficient;

}

void Polynomial::Show_degree(){
    std::cout<<"degree = "<<degree<<std::endl;
}

int Polynomial::Get_degree(){
    return degree;
}

double *Polynomial::Get_coeff(){
    return coefficient;
}

void Polynomial::Show_coefficient(){

    cout<<"coefficient = [ ";

    for(int i=0;i<degree;i++){
        std::cout<<coefficient[i]<<", ";   
    }

    cout<<"]"<<endl;

}


Polynomial Polynomial::Addition(Polynomial p1, Polynomial p2){


    //p1.Show_coefficient();
    //p2.Show_coefficient();

    int deg1 = p1.Get_degree();
    int deg2 = p2.Get_degree();

    int deg = (deg1 > deg2) ? deg1 : deg2;

    double *coef;

    double *ptr1 = p1.Get_coeff();
    double *ptr2 = p2.Get_coeff();


    coef = new double[deg];

   // std::cout<<"coef[";

    for(int i = 0; i < deg; i++){
        coef[i] = (i < deg1) ? ptr1[i] : 0.0;
        coef[i] += (i < deg2) ? ptr2[i] : 0.0;
    }

  
    Polynomial pout(deg,coef);
    delete[] coef;
    delete[] ptr1;
    delete[] ptr2;

    return pout;


}

Polynomial Polynomial::Multiplication(Polynomial p1,Polynomial p2){

    int deg1 = p1.Get_degree();
    int deg2 = p2.Get_degree();

    int deg = deg1 + deg2 - 1;

    double *coef;
    coef = new double[deg];

    double *ptr1 = p1.Get_coeff();
    double *ptr2 = p2.Get_coeff();

    double sum;

    for(int k=0; k<deg; k++){

        sum = 0.0;

        for(int i=0;i<deg1;i++){
            if(k-i<deg2)sum +=ptr1[i]*ptr2[k-i];
        }

        coef[k] = sum;

    }

    Polynomial pout(deg,coef);

    delete[] coef;
    delete[] ptr1;
    delete[] ptr2;

    return pout;

}
