OBJS	= main.o calfun.o getact.o lincoa.o lincob.o prelim.o qmstep.o trstep.o update.o

# FC	= ifort
# FC	= gfortran
FC	= f95

# FFLAGS	= -g 
FFLAGS	= -O

lincoa:	$(OBJS) 
	$(FC) $(FFLAGS) $(OBJS) -o lincoa
clean:
	- rm -f *.o
.f.o:
	$(FC) $(FFLAGS) -c $*.f
