CC ?= gcc

SRC := lsha1.c lua-crypt.c

CFLAGS := -g -o2 -Wall -std=c99

../crypt.so: $(SRC)
	$(CC) $(CFLAGS) -I./ --shared -o $@ $^
	
clean:
	rm -rf ../crypt.so
	

	
	