#include <iostream>
#include <iomanip>
using namespace std;
#include <complex>
#define N 4
#define Iteration 30
complex<double> z[N];

complex<double> func(complex<double> z){
	return (((z-10.0)*z+35.0)*z-50.0)*z+24.0;
}

complex<double> dfunc(complex<double> zi){
	complex<double> sum(1.0 ,0.0);

	for(int i=0;i<N;i++){
		if(zi!=z[i])sum *=(zi-z[i]);
	}

	return sum;


}

int main(void){


	complex<double> z0[N];
	int k=0;

	for(int i=0;i<N;i++){

		double t0,t1;
		cout<<"x0["<<i<<"].Re ->";
		cin>>t0;
		cout<<"x0["<<i<<"].Im ->";
		cin>>t1;

		z0[i] = complex<double>(t0, t1);
		cout<<endl;

	}

	while(k <= Iteration){

		for(int i=0;i<N;i++){
		
			z[i] = z0[i]-func(z0[i])/dfunc(z0[i]);

		}

		k++;
		cout<<setw(3)<<k<<",";
		for(int i=0;i<N;i++){
			z0[i]=z[i];
			cout<<right<<setw(4)<<"z["<<i<<"]="<<right<<setw(23)<<z0[i]<<",";

		}

		cout<<endl;

	}


	return 0;

}