import {describe, expect, it} from "@jest/globals"

describe("sum", () =>{
    it("should add 1 + 2 to return 3", () =>{
        expect(1 + 2).toBe(3);
    })

    it("should add 2 + 3 to return 5", () =>{
        expect(2 + 3).toBe(5);
    })
})