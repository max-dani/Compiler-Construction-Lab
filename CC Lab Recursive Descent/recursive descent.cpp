#include <iostream>
#include <cstring>
#include <cctype>
#include <cstdlib>

using namespace std;

typedef struct treenode *tree;
struct treenode {
    char info;
    tree left;
    tree right;
};

static int i = 0;
char nextsym(char input[]), nextSymbol;
char input[10];

tree treebuild(char x, tree a, tree b);
tree proc_e();
tree proc_t();
tree proc_v();
void inorder(tree root);

int main() {
    tree root;
    int len;
    
    cout << "Enter an expression: ";
    cin >> input;
    len = strlen(input);
    nextSymbol = nextsym(input);
    
    root = proc_e();
    
    if (len != i - 1) {
        cout << "Error" << endl;
        exit(0);
    } else {
        cout << "It's a valid expression\n";
        inorder(root);
    }
    return 0;
}

tree treebuild(char x, tree a, tree b) {
    tree t = new treenode;
    t->info = x;
    t->left = a;
    t->right = b;
    return t;
}

tree proc_e() {
    tree a, b;
    a = proc_t();
    while (nextSymbol == '+' || nextSymbol == '-') {
        if (nextSymbol == '+') {
            nextSymbol = nextsym(input);
            b = proc_t();
            a = treebuild('+', a, b);
        } else {
            nextSymbol = nextsym(input);
            b = proc_t();
            a = treebuild('-', a, b);
        }
    }
    return a;
}

tree proc_t() {
    tree a, b;
    a = proc_v();
    while (nextSymbol == '*' || nextSymbol == '/') {
        if (nextSymbol == '*') {
            nextSymbol = nextsym(input);
            b = proc_v();
            a = treebuild('*', a, b);
        } else {
            nextSymbol = nextsym(input);
            b = proc_v();
            a = treebuild('/', a, b);
        }
    }
    return a;
}

tree proc_v() {
    tree a;
    if (isalpha(nextSymbol)) {
        a = treebuild(nextSymbol, NULL, NULL);
    } else {
        cout << "Error: Invalid character" << endl;
        exit(0);
    }
    nextSymbol = nextsym(input);
    return a;
}

char nextsym(char input[]) {
    i++;
    return input[i - 1];
}

void inorder(tree t) {
    if (t != NULL) {
        inorder(t->left);
        cout << t->info << " ";
        inorder(t->right);
    }
}

