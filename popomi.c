#include <stdio.h>
#include <stdlib.h>
#include <time.h>

int oranges = 0;

void clearInputBuffer() {
    int c;

    while ((c = getchar()) != '\n' && c != EOF);
}

int main(void) {
    srand(time(NULL));

    while (1) {
        printf("Popomi simulator\n\n1. Feed an orange\n2. Roll a dice\nX. Quit\n\nChoose one: ");

        int c = getchar();

        if (c == '\n') {
            continue;
        }

        switch (c) {
            case '1':
                printf("Looks juicy... Popo is happy!\n\nTotal oranges eaten: %d\n", ++oranges);

                break;

            case '2':
                if ((rand() % 100) < 60) {
                    printf("Arrrgh, m-my wrist...!\n");
                } else {
                    printf("You rolled a %d!\n", (rand() % 6) + 1);
                }

                break;

            case 'x':
            case 'X':
                return 0;

            default:
                break;
        }

        clearInputBuffer();
        printf("\n");
    }
}
