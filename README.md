# Cygnus Interpreter
A simple interpreter for a script language Cygnus, written in C#
##What is Cygnus?
Cygnus is a procedure-oriented, light weight, and interactive programming language, written in C#.
Users can easily extend its functions by importing C# functions, and maybe embed it into C# project.
## Basic Type
### Constant
	Cygnus constant can represent integer, double, Boolean,and string. 
	It doesn't support char type currently. 
	To declare a string, you may use "" or '' to enclose its content.
	Example 1:
		x = 12
		y = 23.1
		z = true
		greeting = 'Hello Cygnus!'
### Array
	Array in Cygnus could contain elements from different data types. 
	To declare an array, you may use function 'array' or use '{', '}' to initialize your array.
	The index of an array starts from zero, the same as in C#.
	Example 2:
		arr = array(10)
		arr = {1,2,3,'nice','day'}
		print(a[3]+a[4])
### List
	List could also contain elements from various types. 
	You need to apply function 'list' to have a new list. 
	If you input an array into it, you can initialize your list with the elements in your array.
	Example 3:
		mylist = list()
		mylist = list(1,2,3,'nice','day')
		print(mylist[0])
### Dictionary
	Dictionary is another important data type. 
	The key of dictionary must be an constant, and no restriction for the data type of the values. 
	To initialize a dictionary, you need use function 'dict'. 
	If you input an array with key-value pairs, the dictionary will be initialized with these elements.
	Example 4:
		mydict = dict()
		mydict = dict({'John',21},{'Marry',27})
		mydict['Martin'] = 35
		print(mydict['John'])
## Control Statement
The grammar of the control statement is inspired by Lua, Python and Pascal. 
Every structure here needs an 'end' for closure.
### If
	In Cygnus, there are two types of decision structure. 
	One is 'If-Then', and another is 'If-Then-Else'. 
	There is no 'elseif' or 'elif' in Cygnus. 
	Example 5:
		if a > b then 
			print(a)
		end
		
		if a > b then
			print(a)
		else
			print(b)
		end
### While
	While statement is really useful when you need a loop which will continuously execute under the condition given.
	You can easily learn how to use while loop in Cygnus by the following example:
	Example 6:
		i = 0
		while i < 10 do
			print(i)
			i = i + 1
		end
### For 
	The 'for' loop in Cygnus is like 'foreach' in C#. 
	The grammar is inspired by 'for' in Python. 
	Let's  take a look at this example:
	Example 7:
		for i in range(10) do
			print(i)
		end
	The 'range' function here is a lazy function, which is the same as 'xrange' in Python. 
	So don't worry about generating a large set for the loop, it won't :)
	Also,  you may use the for loop to traverse every element in a collection.
	Example 8:
		sum = 0
		for i in {1,2,3} do
			sum = sum + i
		end
### Break
	You may need a break if you want to jump out of a loop
	Example 9:
		for i in range(10) do
			if i > 5 then
				break
			else
				print(i)
			end
		end
### Return
	Keyword 'return' can let you go out of the function. 
	Refer to example 10.
## Function
### Define a function
	Use keyword 'def' to define a function. 
	Notice that the function has its own scope. 
	The variables in the functions could be out of the functions' scope,
	which means the function can contain global variables.
	Example 10:
		def max(x, y) begin
			if x > y then
				return x
			else
				return y
			end
		end
### Higher-order function
	Function in Cygnus can be used as a parameter, 
	namely you can write higher-order functions. 
	See the following example:
	Example 11:
		def mul(x,y) begin
			return x * y
		end
		def mul2(f,x) begin
			return f(x,2)
		end
		print(mul2(mul,10))
	Nevertheless, the lambda expression is not supported in Cygnus currently.

## Advanced Features
### Table
	The table in Cygnus are designed for being used as a mini class :), which enables users to define a new data type.
	It is capable of inheriting, but it can only have no more than one parent table.
	Example 12:
		p = table(age,job)
		p.age = 23
		p.job = 'doctor'
		setparent(p,table(city))
	I recommend writing a constructor for the table, which makes the code more readable.
	Example 13:
		def person(age,job) begin
			p = table(age,job)
			p.age = age
			p.job = job
			return p
		end
		p1 = person(23,'engineer')
## Cygnus Code Examples
### Find the oldest person
	def person(name,age) begin 
    	p = table(name,age)
    	p.name = name
    	p.age = age
    	return p
	end

	people = {
    	person('shao',23),
        person('wang',20),
        person('zhang',21),
        person('he',22)
    }
	max_age = -1
	max_name = ''
	for p in people do
    	if max_age < p.age then 
        	max_age = p.age
        	max_name = p.name
    	end
	end
	result = strformat('name: {0}  age: {1}',max_name,max_age)
	print(result)
### Create a linkedlist
	def linkednode(value,previous,next) begin
		node = table(value,previous,next)
		node.value = value
		node.previous = previous
		node.next = next
		return node
	end

	def linkedlist() begin
		mylist = table(first,last,count)
		mylist.first = null
		mylist.last = null
		return mylist
	end

	def link_add(mylist,value) begin
		if mylist.first == null then
			node = linkednode(value,null,null)
			mylist.first = node
			mylist.last = node
		else
			node = linkednode(value,mylist.last,null)
			mylist.last.next = node
			mylist.last = node
		end
		return void
	end

	def link_disp(mylist) begin
		current = mylist.first
		while current != null do
			print(current.value)
			current = current.next
		end
		return void
	end
	
	mylist = linkedlist()
	link_add(mylist,12)
	link_add(mylist,24)
	link_add(mylist,36)
	link_add(mylist,48)
	link_disp(mylist)

## Built-in functions
### Basic Functions
* print(object): print the object
	
* array(n): initialize an array
	
* list(args): initialize a list
	
	dict(args): initialize a dictionary
	
	table(args): initialize a table
	
	setparent(table,parent_table): set a table's parent table
	
	length(set): return the length of a set,e.g. array,list...
	
	import(dll_path,namespace_and_class): import C# functions from a dll file
	
	execfile(file_path): execute code from a file
	
	throw(error): throw an error
	
	delete(args): delete variables
	
	scan(str): print a sentence in the console, wait for the input, and return the input as string
	
	range(start, stop, step): generate a sequence
	
	exit(): exit from the programm

