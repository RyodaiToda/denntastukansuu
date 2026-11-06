#ifndef __Polynomial__
#define __Polynomial__

class Polynomial{

	public :
		Polynomial();
		Polynomial(int n);
		Polynomial(int n,double coef[]);
		~Polynomial();

		void Show_degree();
		void Show_coefficient();

		static Polynomial Addition(Polynomial p1, Polynomial p2);
		static Polynomial Multiplication(Polynomial p1,Polynomial p2);

		int Get_degree();
		double *Get_coeff();

	private:
		int degree;
		double *coefficient;

};

#endif