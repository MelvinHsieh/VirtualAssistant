const request = require('supertest')
const mockingoose = require('mockingoose');
const User = require("../models/user")

describe('User service', () => {
    describe('Fetch Users', () => {
        it('should return the list of User', async () => {
            mockingoose(User).toReturn([
                { username: "TestUser", password: "test", roles: ["role1"] },
                { username: "TestUser2", password: "test2", roles: ["role1", "role2"] },
            ], 'find');
            const results = await User.find({}).exec();
            expect(results.length).toBe(2);
        });
    });
});