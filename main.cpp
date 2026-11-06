#define _CRTDBG_MAP_ALLOC
#include "Polynomial.hpp"
#include <iostream>
#include <crtdbg.h>
using namespace std;

int main() {

    double array1[] = {-1,1};
    Polynomial p1(2,array1);

    double array2[] = {1,1,1,1,1,1,1};
    Polynomial p2(7,array2);

    Polynomial p3 = Polynomial::Addition(p1,p2);
    Polynomial p4 = Polynomial::Multiplication(p1,p2);

    cout<<"Addition"<<endl;
    p3.Show_degree();
    p3.Show_coefficient();

    cout<<endl;


    cout<<"Multiplication"<<endl;
    p4.Show_degree();
    p4.Show_coefficient();

    cout<<endl;


    //_CrtSetReportMode(_CRT_WARN, _CRTDBG_MODE_DEBUG); 
    //_CrtDumpMemoryLeaks();
    return 0;
}